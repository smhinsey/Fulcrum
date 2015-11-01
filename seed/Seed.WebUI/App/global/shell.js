angular.module('fulcrumSeed.global.shell', [])
	.controller('navController', [
		'$scope', '$state', '$rootScope',
		function($scope, $state, $rootScope) {

			$rootScope.$on('authenticated', function() {
				$scope.visible = true;

				if (!$scope.$$phase) {
					$scope.$apply();
				}
			});

			$rootScope.$on('unauthenticated', function() {
				$scope.visible = false;

				if (!$scope.$$phase) {
					$scope.$apply();
				}
			});

			$scope.visible = false;
		}
	])
	.controller('badgeController', [
		'$scope', '$state', 'authSvc', '$rootScope', 'authRedirectSvc', '$modal',
		function($scope, $state, authSvc, $rootScope, authRedirectSvc, $modal) {

			$scope.visible = false;

			$rootScope.$on('authenticated', function() {
				//profileSvc.get()
				//	.then(function(profile) {
				//			$scope.profile = profile;
				//			$scope.visible = true;
				//if (!$scope.$$phase) {
				//	$scope.$apply();
				//}
				//		},
				//		function(error) {
				//			// TODO: ??
				//		});
			});

			$rootScope.$on('unauthenticated', function() {
				$scope.visible = false;
				$scope.profile = undefined;
			});

			$scope.logout = function() {
				$scope.visible = false;
				authSvc.logout();
				authRedirectSvc.clearRedirect();
				$rootScope.$broadcast("unauthenticated");
				$state.go("home");
			};

			$scope.login = function() {

				var loginModal = $modal.open({
					templateUrl: 'login.html',
					controller: 'loginController',
					size: 'sm',
				});

			};
		}
	])
	.controller('loginController', [
		'$scope', '$state', 'authSvc', '$rootScope', 'authRedirectSvc', '$modal',
		function($scope, $state, authSvc, $rootScope, authRedirectSvc, $modal) {

			$scope.username = "";
			$scope.password = "";

			$scope.login = function () {

				console.log("Login attempt");
				console.log("Username:", $scope.email);
				console.log("Password:", $scope.password);

				authSvc.login($scope.email, $scope.password)
					.then(function() {
						console.log('log in succeeded');
					}, function() {
						console.log("log in failed");
					});
			};
		}
	]);