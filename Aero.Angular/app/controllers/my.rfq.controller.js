aero.controller('MyRfqCtrl',
    ['$scope', 'Q', 'amplify', 'datacontext', 'AeroStore', 'logger',
    function ($scope, Q, amplify, datacontext, AeroStore, logger) {

        $scope.rfqs = [];
        $scope.customerId;
        InitMyRfq = function () {
             AeroStore.getCustomer()
                .then(function(customerId){
                    $scope.customerId = customerId;
                    datacontext.loadMyRfq(customerId, $scope.pageSize(), $scope.skipSize())
                        .then(function(data){
                            $scope.rfqs = data.results;
                            refreshView();
                        })
                });
            
        };

    	$scope.gridOptions = {
    	    data: 'rfqs',
    		columnDefs: [
                { field: 'dateSubmitted', displayName: 'Date', width: '25%' },
                { field: 'part.partNumber', displayName: 'Part Number', width: '25%' },
                { field: 'model', displayName: 'Model', width: '20%' },                
                { field: 'status', displayName: 'Status', width: '10%' },
    		],
    		multiSelect: false,
    		enablePaging: true,
    		showFooter: false,
    		pagingOptions: $scope.pagingOptions,
    		totalServerItems: '0',
    		selectedItems: $scope.selectedRfq,
    		plugins: [new ngGridFlexibleHeightPlugin()]
    	};

    	$scope.selectedRfq = [];

    	$scope.pagingOptions = {
    	    pageSizes: [10, 20, 30],
    	    pageSize: 20,
    	    currentPage: 1
    	};

    	$scope.noOfPages = 0;
    	$scope.currentPage = 1;
    	$scope.maxSize = 9;

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

    	var triggerRfqLoad = true;
    	$scope.$watch('pagingOptions', function (newVal, oldVal) {
    	    if (newVal !== oldVal) {
    	        $scope.loadRfq();
    	    }
    	}, true);

    	$scope.$watch('currentPage', function (newVal, oldVal) {
    	    if (newVal !== oldVal && triggerSearch) {
    	        $scope.loadRfq();
    	    }
    	}, true);

    	function setCurrentPage(page) {
    	    triggerRfqLoad = false;
    	    $scope.currentPage = page;
    	    triggerRfqLoad = true;
    	}

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

    	$scope.loadRfq = function() {
    	    $('#myRfqGrid').hide();
    	    datacontext.loadMyRfq($scope.customerId, $scope.pageSize(), $scope.skipSize())
                .then(getSucceeded).fail(failed)
                .fin(function () {
                    $('#myRfqGrid').css(startValues);
                    $('#myRfqGrid').animate(endValues, 400, 'swing');
                    refreshView();
                }).done();
    	};

    	function getSucceeded(data) {
    	    $scope.rfqs = data.results;
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

    	InitMyRfq();

}]);