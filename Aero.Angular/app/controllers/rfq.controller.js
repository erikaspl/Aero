aero.controller('RfqCtrl',
    ['$scope', '$location', 'amplify', 'breeze', 'datacontext', 'logger', 'AeroStore',
    function ($scope, $location, amplify, breeze, datacontext, logger, AeroStore) {

        logger.log("creating RfqCtrl");
        $scope.partId = amplify.store('selectedPartId');
        $scope.part;
        $scope.quantities = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

        datacontext.createEntity("RFQ").then(function (newRfq) {
            $scope.rfq = newRfq;
        });

        datacontext.loadRfqPriorities().then(function(data){
            $scope.priorities = data;
        });

        $scope.loadPart = function (partId) {
            datacontext.loadPart(partId)
                .then(getSucceeded)
                .fail(failed)
                .fin(finish);
        };


        $scope.submitRfq = function () {
            var customerId = AeroStore.getCustomer()
                .then(function (customerId) {
                    $scope.rfq.customerId = customerId;
                    $scope.rfq.partId = $scope.part.id;
                    datacontext.saveEntity($scope.rfq)
                        .then(function(){
                            $scope.close();
                        });
                    //console.log('submitRfq for customer ' + customerId);
                });            
        };


        function getSucceeded(data) {
            $scope.part = data;
        };

        function failed(error) {
            $scope.error = error.message;
        };

        function finish() {
            refreshView();
        };

        function refreshView() {
            $scope.$apply();
        };

        $scope.$on('openRfqModal', function () {
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
            template: 'app/views/rfq.view.html',
            persist: true,
            controller: 'RfqCtrl',
            show: false,
            backdrop: 'static',
            socpe: $scope
        };



        $scope.loadPart($scope.partId);

    }]);