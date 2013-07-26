/* main: startup script creates the 'aero' module and adds custom Ng directives */

// 'aero' is the one Angular (Ng) module in this app
// 'aero' module is in global namespace
window.aero = angular.module('aero', ['ngCookies', 'ngGrid', 'ngLocale', 'ui.bootstrap', 'ui.bootstrap.dialog', '$strap.directives']);

// Add global "services" (like breeze and Q) to the Ng injector
// Learn about Angular dependency injection in this video
// http://www.youtube.com/watch?feature=player_embedded&v=1CpiB3Wk25U#t=2253s
aero.value('breeze', window.breeze)
    .value('Q', window.Q)
    .value('_', window._)
    .value('amplify', window.amplify)
    .value('moment', window.moment)
    .value('toastr', window.toastr);

// Configure routes
aero.config(['$routeProvider', '$locationProvider', '$httpProvider', function ($routeProvider, $locationProvider, $httpProvider) {

    var access = routingConfig.accessLevels;

    $routeProvider.
          when('/', { templateUrl: 'app/views/search.view.html', controller: 'SearchCtrl' }).
          when('/about', { templateUrl: 'app/views/about.view.html', controller: 'AboutCtrl' }).
          when('/parts', { templateUrl: 'app/views/search.view.html', controller: 'SearchCtrl' }).
          when('/rfq', { templateUrl: 'app/views/rfq.view.html', controller: 'RfqCtrl' }).
          when('/myrfq', { templateUrl: 'app/views/my.rfq.view.html', controller: 'MyRfqCtrl' }).
          otherwise({ redirectTo: '/404' });
  }]);

//#region Ng directives
/*  We extend Angular with custom data bindings written as Ng directives */
aero.directive('onFocus', function () {
        return {
            restrict: 'A',
            link: function (scope, elm, attrs) {
                elm.bind('focus', function () {
                    scope.$apply(attrs.onFocus);
                });
            }
        };
    })
    .directive('onBlur', function () {
        return {
            restrict: 'A',
            link: function (scope, elm, attrs) {
                elm.bind('blur', function () {
                    scope.$apply(attrs.onBlur);
                });
            }
        };
    })
    .directive('onEnter', function () {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if (event.which === 13) {
                    scope.$apply(function () {
                        scope.$eval(attrs.onEnter);
                    });

                    event.preventDefault();
                }
            });
        };
    })
    .directive('selectedWhen', function () {
        return function (scope, elm, attrs) {
            scope.$watch(attrs.selectedWhen, function (shouldBeSelected) {
                if (shouldBeSelected) {
                    elm.select();
                }
            });
        };
    });
if (!Modernizr.input.placeholder) {
    // this browser does not support HTML5 placeholders
    // see http://stackoverflow.com/questions/14777841/angularjs-inputplaceholder-directive-breaking-with-ng-model
    aero.directive('placeholder', function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attr, ctrl) {

                var value;

                var placeholder = function () {
                    element.val(attr.placeholder);
                };
                var unplaceholder = function () {
                    element.val('');
                };

                scope.$watch(attr.ngModel, function (val) {
                    value = val || '';
                });

                element.bind('focus', function () {
                    if (value == '') unplaceholder();
                });

                element.bind('blur', function () {
                    if (element.val() == '') placeholder();
                });

                ctrl.$formatters.unshift(function (val) {
                    if (!val) {
                        placeholder();
                        value = '';
                        return attr.placeholder;
                    }
                    return val;
                });
            }
        };
    });
}
//#endregion 