aero.controller('RfqCtrl',
    ['$scope', '$location', 'amplify', 'model', 'breeze', 'datacontext', 'logger', 'toastr', 'AeroStore',
    function ($scope, $location, amplify, model, breeze, datacontext, logger, toastr, AeroStore) {

        logger.log("creating RfqCtrl");
        $scope.partId = AeroStore.getSelectedPart();
        $scope.part;
        $scope.quantities = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

        datacontext.loadRfqPriorities().then(function(data){
            $scope.priorities = data;
        }).fail(function (error) {
            console.log(error)
        });

        datacontext.createEntity("RFQ").then(function (newRfq) {
            $scope.rfq = newRfq;
        }).fail(function (error) {
            console.log(error)
        });

        $scope.loadPart = function (partId) {
            datacontext.loadPart(partId)
                .then(getSucceeded)
                .fail(failed)
                .fin(finish);
        };

        $scope.$watch('partId', function (newVal, oldVal) {
            if (newVal !== oldVal) {
                $scope.loadPart(newVal);
            }
        }, true);


        $scope.submitRfq = function () {
            var customerId = AeroStore.getCustomer()
                .done(function (customerId) {
                    $scope.rfq.customerId = customerId;
                    $scope.rfq.partId = $scope.part.id;
                    datacontext.saveEntity($scope.rfq)
                        .then(function (message) {
                            $scope.close();
                            toastr.success("Your request for qutation has been saved.");
                            finish();
                        }, function () {
                            $scope.close();
                            toastr.error("Unable to send your request.");
                            finish();
                        });
                });            
        };


        function getSucceeded(data) {
            $scope.part = data;
        };

        function failed(error) {
            $scope.error = error.message;
            logger.log(error);
        };

        function finish() {
            refreshView();
        };

        function refreshView() {
            $scope.$apply();
        };

        $scope.$on('openRfqModal', function () {
            model.rfqInitializer($scope.rfq);
            $scope.partId = AeroStore.getSelectedPart();
            $scope.open();
        });

        $scope.open = function () {
            console.log('open modal');
            $scope.shouldBeOpen = true;
        }

        $scope.close = function () {
            console.log('close modal');
            $scope.shouldBeOpen = false;
        }

        $scope.modalopts = {
            backdropFade: true,
            dialogFade: false
        };
    }]);