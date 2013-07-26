/* datacontext: data access and model management layer */

// create and add datacontext to the Ng injector
// constructor function relies on Ng injector
// to provide service dependencies
aero.factory('datacontext',
    ['breeze', 'Q', 'model', 'logger', '$timeout', '_',
    function (breeze, Q, model, logger, $timeout, _) {

        logger.log("creating datacontext");
        var initialized;

        configureBreeze();

        var jsonResultsAdapter = new breeze.JsonResultsAdapter({
            name: "getByIdAdapter",
            extractResults: function (json) {
                return json;
            },
            visitNode: function (node, queryContext, nodeContext) {
                return {
                    id: node.$id
                };
            }
        });

        var manager = new breeze.EntityManager("/api/Aero");
        manager.enableSaveQueuing(true);

        var datacontext = {
            metadataStore: manager.metadataStore,
            searchParts: searchParts,
            loadPart: loadPart,
            loadRfqPriorities: loadRfqPriorities,
            loadMyRfq: loadMyRfq,
            fetchMetadata: fetchMetadata,
            getCustomerByUserName: getCustomerByUserName,
            createEntity: createEntity,
            saveEntity: saveEntity
        };
        model.initialize(datacontext);
        return datacontext;

        function searchParts(partNumber, takeSize, skipSize) {
            var query = breeze.EntityQuery
                .from("Parts")
                .where("partNumber", "Contains", partNumber)
                .orderBy("partNumber")
                .take(takeSize)
                .skip(skipSize)
                .inlineCount();

            return manager.executeQuery(query)
                .then(getSucceededFullRez)
                .then(function (data) {
                    data.pageSize = takeSize;
                    return data;
                }); // caller to handle failure
        }

        function loadPart(partId) {
            var query = breeze.EntityQuery
                .from("Parts")
                .where("id", "==", partId);

            return manager.executeQuery(query)
                .then(getSucceededSingleRez); 
        }

        function loadRfqPriorities(forceRefresh) {
            var query = breeze.EntityQuery
                .from("Priorities");

            if (initialized && !forceRefresh) {
                query = query.using(breeze.FetchStrategy.FromLocalCache);
            }
            initialized = true;

            return manager.executeQuery(query)
                .then(getSucceeded)
                .then(function (data) {
                    return data;
                }); // caller to handle failure
        }

        function loadMyRfq(customerId, takeSize, skipSize) {
            var query = breeze.EntityQuery
                .from("RFQs")
                .where("customerId", "==", customerId)
                .expand("part")
                //.orderBy("dateSubmitted")
                .take(takeSize)
                .skip(skipSize)
                .inlineCount();

            return manager.executeQuery(query)
                .then(function (data) {
                    return getSucceeded(data);
                }).done(); // caller to handle failure
        }


        //#region private members

        function createEntity(entityName) {
            var defer = Q.defer();

            fetchMetadata().then(function () {
                var entity = manager.createEntity(entityName);
                defer.resolve(entity);
            });

            return defer.promise;
        }

        function fetchMetadata() {
            var defer = Q.defer();

            if (manager.metadataStore.isEmpty()) {
                manager.metadataStore.fetchMetadata(manager.dataService)
                    .then(function (rawMetadata) {
                        defer.resolve();
                    });
            } else {
                defer.resolve();
            }

            return defer.promise;
        }

        function getCustomerByUserName(userName, forceRefresh) {
            var query = breeze.EntityQuery
                            .from("Customers")
                            .where("userName", "==", userName);

            if (initialized && !forceRefresh) {
                query = query.using(breeze.FetchStrategy.FromLocalCache);
            }
            initialized = true;

            return manager.executeQuery(query)
                .then(getSucceededSingleRez)
                .then(function (data) {
                    return data;
                }); // caller to handle failure
        };

        function getTodoLists(forceRefresh) {

            var query = breeze.EntityQuery
                .from("TodoLists")
                .expand("Todos")
                .orderBy("todoListId desc");

            if (initialized && !forceRefresh) {
                query = query.using(breeze.FetchStrategy.FromLocalCache);
            }
            initialized = true;

            return manager.executeQuery(query)
                .then(getSucceeded); // caller to handle failure
        }

        function getSucceededFullRez(data) {
            var qType = data.XHR ? "remote" : "local";
            logger.log(qType + " query succeeded");
            return data;
        }

        function getSucceeded(data) {
            var qType = data.XHR ? "remote" : "local";
            logger.log(qType + " query succeeded");
            return data.results;
        }

        function getSucceededSingleRez(data) {
            var qType = data.XHR ? "remote" : "local";
            logger.log(qType + " query succeeded");
            return _.first(data.results);
        }

        function createTodoItem() {
            return manager.createEntity("TodoItem");
        }

        function createTodoList() {
            return manager.createEntity("TodoList");
        }

        function deleteTodoItem(todoItem) {
            todoItem.entityAspect.setDeleted();
            return saveEntity(todoItem);
        }

        function deleteTodoList(todoList) {
            // Neither breeze nor server cascade deletes so we have to do it
            var todoItems = todoList.todos.slice(); // iterate over copy
            todoItems.forEach(function (entity) { entity.entityAspect.setDeleted(); });
            todoList.entityAspect.setDeleted();
            return saveEntity(todoList);
        }

        function saveEntity(masterEntity) {
            var defer = Q.defer();
            // if nothing to save, return a resolved promise
            if (!manager.hasChanges()) {
                return defer.resolve();
            }

            var description = describeSaveOperation(masterEntity);
            return manager.saveChanges().then(saveSucceeded).fail(saveFailed);

            function saveSucceeded() {
                var successMsg = "saved " + description;
                logger.log(successMsg);
                return defer.resolve(successMsg);
            }

            function saveFailed(error) {
                var msg = "Error saving " +
                    description + ": " +
                    getErrorMessage(error);

                masterEntity.errorMessage = msg;
                logger.log(msg, 'error');
                // Let user see invalid value briefly before reverting
                $timeout(function () { manager.rejectChanges(); }, 1000);
                throw error;
            }

            return defer.promise();
        }
        function describeSaveOperation(entity) {
            var statename = entity.entityAspect.entityState.name.toLowerCase();
            var typeName = entity.entityType.shortName;
            var title = entity.title;
            title = title ? (" '" + title + "'") : "";
            return statename + " " + typeName + title;
        }
        function getErrorMessage(error) {
            var reason = error.message;
            if (reason.match(/validation error/i)) {
                reason = getValidationErrorMessage(error);
            }
            return reason;
        }
        function getValidationErrorMessage(error) {
            try { // return the first error message
                var firstItem = error.entitiesWithErrors[0];
                var firstError = firstItem.entityAspect.getValidationErrors()[0];
                return firstError.errorMessage;
            } catch (e) { // ignore problem extracting error message 
                return "validation error";
            }
        }

        function configureBreeze() {
            // configure to use the model library for Angular
            breeze.config.initializeAdapterInstance("modelLibrary", "backingStore", true);

            // configure to use camelCase
            breeze.NamingConvention.camelCase.setAsDefault();

            // configure to resist CSRF attack
            var antiForgeryToken = $("#antiForgeryToken").val();
            if (antiForgeryToken) {
                // get the current default Breeze AJAX adapter & add header
                var ajaxAdapter = breeze.config.getAdapterInstance("ajax");
                ajaxAdapter.defaultSettings = {
                    headers: {
                        'RequestVerificationToken': antiForgeryToken
                    },
                };
            }
        }
        //#endregion
    }]);