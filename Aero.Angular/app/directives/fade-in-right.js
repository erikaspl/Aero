aero.directive('fadeInRight', function () {
    var directiveDefinitionObject = {
        restrict: 'A',
        link: function ($scope, element, attributes) {

            var isVisible = attributes.fadeInRight;
            var duration = (attributes.fadeInDuration || "fast");

            if (!$scope.$eval(isVisible)) {
                element.hide();
            }

            $scope.$watch(isVisible, function (oldVal, newVal) {
                if (oldVal === newVal) {
                    return;
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

                var displayNone = {
                    opacity: 0
                };

                if (newVal) {
                    $(element).css(startValues);
                    $(element).animate(endValues, duration, 'swing');
                } else {
                    $(element).css(displayNone);
                }
            }, true);
            
        }
    };

    return directiveDefinitionObject;    
});