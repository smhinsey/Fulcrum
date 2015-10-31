angular.module('fulcrumSeed.global.shell', [])
	.controller('navController', [
		'$scope', '$state', '$rootScope',
		function ($scope, $state, $rootScope) {

			$rootScope.$on('authenticated', function () {
				$scope.visible = true;

				if (!$scope.$$phase) {
					$scope.$apply();
				}
			});

			$rootScope.$on('unauthenticated', function () {
				$scope.visible = false;

				if (!$scope.$$phase) {
					$scope.$apply();
				}
			});

			$scope.visible = false;
		}
	])
	.controller('badgeController', [
		'$scope', '$state', 'authSvc', 'profileSvc', '$rootScope', 'authRedirectSvc', '$timeout',
		function ($scope, $state, authSvc, profileSvc, $rootScope, authRedirectSvc, $timeout) {

			$scope.visible = false;

			$rootScope.$on('authenticated', function () {
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

			$rootScope.$on('unauthenticated', function () {
				$scope.visible = false;
				$scope.profile = undefined;
			});

			$scope.logout = function () {
				$scope.visible = false;
				authSvc.logout();
				authRedirectSvc.clearRedirect();
				$rootScope.$broadcast("unauthenticated");
				$state.go("home");
			};
		}
	])