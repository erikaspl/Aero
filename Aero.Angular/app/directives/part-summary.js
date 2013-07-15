aero.directive('partSummary', function () {

    var directiveDefinitionObject = {
        restrict: 'E',
        templateUrl: 'templates/part-summary.html',
        replace: true,
        scope: {
            part: '=',
            rfq: '=',
            quantities: "=",
            priorities: "=",
            submitRfq: "&"
        }
    };

    return directiveDefinitionObject;
});