aero.controller('SearchCtrl',
    ['$scope', '$location', '$dialog', 'Q', 'breeze', 'amplify', 'datacontext', 'logger',
    function ($scope, $location, $dialog, Q, breeze, amplify, datacontext, logger) {

        logger.log("creating SearchCtrl");
        $scope.searchPartText = "";
        $scope.Parts = [];
        $scope.error = "";
        $scope.totalItems = 0;
        $scope.totalItemsMessage = {
            0: 'no records found',
            one: '{} record found',
            other: '{} records found'
        };

        $scope.pagingOptions = {
            pageSizes: [10, 20, 30],
            pageSize: 20,
            currentPage: 1
        };

        $scope.noOfPages = 0;
        $scope.currentPage = 1;
        $scope.maxSize = 9;
        $scope.hidePagination = function () {
            if ($scope.totalItems <= 0) {
                return true;
            }
            return false;
        }

        $scope.selectedPart = [];
        $scope.poButton = '<button id="editBtn" type="button" class="btn btn-primary btn-small"  ng-click="order(row)" >Order</button> ';
        //$scope.poButton = '<button id="editBtn" type="button" class="btn btn-primary btn-small" bs-modal="\'app/views/rfq.view.html\'">Order</button> ';
        $scope.gridOptions = {
            data: 'Parts',
            columnDefs: [
                { field: 'partNumber', displayName: 'Part Number', width: '25%' },
                { field: 'model', displayName: 'Model', width: '20%' },
                { field: 'description', displayName: 'Description', width: '25%' },
                { field: 'condition', displayName: 'Condition', width: '10%' },
                { field: 'qty', displayName: 'Qty', width: '10%' },
                { displayName: '', cellTemplate: $scope.poButton, width: '10%', cellClass: 'centered' }
            ],
            rowTemplate:'<div ng-style="{\'cursor\': row.cursor, \'z-index\': col.zIndex() }" ng-repeat="col in renderedColumns" ng-class="col.colIndex()" class="ngCell {{col.cellClass}}" ng-cell></div>',
            multiSelect: false,
            enablePaging: true,
            showFooter: false,
            pagingOptions: $scope.pagingOptions,
            totalServerItems: '0',
            selectedItems: $scope.selectedPart,
            plugins: [new ngGridFlexibleHeightPlugin()]
        };

        $scope.modalOpen = false;

        $scope.order = function (row) {
            amplify.store('selectedPartId', row.entity.id);

            $scope.$broadcast('openRfqModal', row);

            //var msgbox = $dialog.messageBox('Delete Item', 'Are you sure?', [{ label: 'Yes, I\'m sure', result: 'yes' }, { label: 'Nope', result: 'no' }]);
            //msgbox.open().then(function (result) {
            //    if (result === 'yes') {
            //        //deleteItem(item);
            //    }
            //});
            //$scope.showModalViaService();
            //$location.search('part', row.entity.id);
            //$location.path("/rfq");
        };

        $scope.skipSize = function () {
            var pageSize = parseInt($scope.pagingOptions.pageSize);
            var skipSize = 0;
            if ($scope.currentPage > 1) {
                skipSize = $scope.currentPage * pageSize;
            }
            return skipSize;
        };

        $scope.pageSize = function () {
            return parseInt($scope.pagingOptions.pageSize);
        };

        var triggerSearch = true;
        $scope.$watch('pagingOptions', function (newVal, oldVal) {
            if (newVal !== oldVal) {
                $scope.searchParts();
            }
        }, true);

        $scope.$watch('currentPage', function (newVal, oldVal) {
            if (newVal !== oldVal && triggerSearch) {
                $scope.searchParts();
            }
        }, true);

        function setCurrentPage(page) {
            triggerSearch = false;
            $scope.currentPage = page;
            triggerSearch = true;
        }

        $scope.searchClick = function() {
            setCurrentPage(1);
            $scope.searchParts();
        };

        var startValues = {
            marginLeft: '20px',
            marginRight: '-20px',
            opacity: 0,
            display: 'block'
        };

        var endValues = {
            marginRight: 0,
            marginLeft: 0,
            opacity: 1
        };

        $scope.partsLoaded = true;
        $scope.searchParts = function () {
            searchPartsHandle();
        };

        function searchPartsHandle() {
            //$scope.$safeApply();

            //$scope.$apply("testVal = 2");
            $('#rezGrid').hide();
            //$scope.$apply(function () {
            //    $scope.partsLoaded = false;
            //});
            datacontext.searchParts($scope.searchPartText, $scope.pageSize(), $scope.skipSize())
                .then(getSucceeded).fail(failed)
                .fin(function () {
                    //$scope.partsLoaded = true;
                    $('#rezGrid').css(startValues);
                    $('#rezGrid').animate(endValues, 400, 'swing');
                    updateSearchCache($scope.searchPartText, $scope.pageSize(), $scope.skipSize(), $scope.currentPage);
                    refreshView();
                }).done();
        };

        function initSearchParams() {
            if (amplify.store('searchText') != null) {
                $scope.searchPartText = amplify.store('searchText');
                $scope.pagingOptions.pageSize = amplify.store('pageSize');
                $scope.pagingOptions.skipSize = amplify.store('skipSize');
                setCurrentPage(amplify.store('currentPage'));
                searchPartsHandle();
            }
        };

        function updateSearchCache(srachText, pageSize, skipSize, currentPage) {
            var expireTime = 600000;
            amplify.store('searchText', srachText, { expires: expireTime });
            amplify.store('pageSize', pageSize, { expires: expireTime });
            amplify.store('skipSize', skipSize, { expires: expireTime });
            amplify.store('currentPage', currentPage, { expires: expireTime });
        };

        function getSucceeded(data) {
            $scope.Parts = data.results;
            $scope.totalItems = data.inlineCount;            
            $scope.noOfPages = GetNoOfPages($scope.totalItems, $scope.pageSize());
        };

        function GetNoOfPages(totalItems, pageSize) {
            var numberOfPages = Math.floor(totalItems / pageSize);
            if (numberOfPages <= 0) {
                numberOfPages = 1;
            }
            return numberOfPages;
        };

        function failed(error) {
            $scope.error = error.message;
        };

        function refreshView() {
            $scope.$apply();
        };

        initSearchParams();
    }]);