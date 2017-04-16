angular.module('ReviewCuratorApp', ['blockUI', 'ngToast'])
    .config(['ngToastProvider', function (ngToastProvider) {
        ngToastProvider.configure({
            horizontalPosition: 'center', // or 'fade',
            verticalPosition: 'top'
        });
    }])
    .config(["blockUIConfig", function (blockUIConfig) {
        blockUIConfig.requestFilter = function (config) {
            //Perform a global, case-insensitive search on the request url for 'noblockui' ...
            if (config.url.match(/noblockui/gi)) {
                return false; // ... don't block it.
            }
        };
    }])
    .controller('ReviewCuratorController', function ($scope, ReviewCuratorService, $window, ngToast) {
        activate();

        function activate() {

        }

        $scope.getReviews = getReviews;

        function getReviews() {
            ReviewCuratorService
                .getReviews($scope.url, $scope.count, $scope.email)
                .then(reviewSuccessFn, reviewErrorFn);

            function reviewSuccessFn(data, status, headers, config) {
                var response = data.data;
                if (response.errorDisplayMessage) {
                    ngToast.create({
                        content: response.errorDisplayMessage,
                        className: 'danger',
                        timeout: '5000'
                    });
                    return;
                }

                if (response.firstFewReviews.length > 0) {
                    $scope.latestReviews = response.firstFewReviews;
                }

                $scope.reviewSource = response.reviewSource;
                $scope.reviewsFileUrl = response.reviewsFileUrl;
                $scope.done = true;
            }

            function reviewErrorFn() {
                ngToast.create({
                    content: "Oops! Could not download reviews at this time. Please try later.",
                    className: 'danger',
                    timeout: '5000'
                });
            }

        }


    })
    .factory('ReviewCuratorService', function ($http) {
        var service = {
            getGreeting: getGreeting,
            getReviews: getReviews
        };

        function getGreeting() {
            return "Hello world";
        }

        function getReviews(url, count, userEmail) {
            return $http.post('api/reviews', { productUrl: url, count: count, email: userEmail });
        }

        return service;
    });