angular.module('fulcrumSeed.global.shell', ['fulcrumSeed.global.directives.profile'])
	.controller('navController', [
		'$scope', '$state', '$rootScope',
		function ($scope, $state, $rootScope) {

			$rootScope.$on('authenticated', function () {
				console.log('authenticated');
				$scope.visible = true;

				if (!$scope.$$phase) {
					console.log('authenticated $$phase');
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
		'$scope', '$state', 'authSvc', '$rootScope', 'authRedirectSvc',
		'$modal', 'profileSvc',
		function ($scope, $state, authSvc, $rootScope, authRedirectSvc,
			$modal, profileSvc) {


			var showLogin = function () {
					$modal.open({
						templateUrl: 'login.html',
						controller: 'loginController',
						size: 'sm',
					});
			};

			$scope.visible = false;

			$rootScope.$on('authenticated', function () {
				profileSvc.getClaims()
					.then(function (response) {
						$scope.claims = response.data;

						profileSvc.setProfile({ claims: $scope.claims });

						_.forOwn($scope.claims, function (value, key) {
							// TODO: extract some of these into a standard profile object?
							//console.log(key);
						});
					},
						function (error) {
							// TODO: ??
						});
			});

			$rootScope.$on('unauthenticated', function () {
				$scope.visible = false;
				$scope.profile = undefined;
			});

			$rootScope.$on('show_login', function () {
				console.log('received show_login');
				showLogin();
			});

			$scope.logout = function () {
				$scope.visible = false;
				authSvc.logout();
				authRedirectSvc.clearRedirect();
				$rootScope.$broadcast("unauthenticated");

				$state.go("home", {}, { reload: true });
				window.location.reload();
			};

			$scope.login = function () {
				showLogin();
			};
		}
	])
	.controller('loginController', [
		'$scope', '$state', 'authSvc', '$rootScope', 'authRedirectSvc', '$modalInstance',
		function ($scope, $state, authSvc, $rootScope, authRedirectSvc, $modalInstance) {

			$scope.username = "";
			$scope.password = "";

			$scope.login = function () {

				//console.log("Login attempt");
				//console.log("Username:", $scope.email);
				//console.log("Password:", $scope.password);

				authSvc.login($scope.email, $scope.password)
					.then(function () {
						$rootScope.$broadcast("authenticated");
						$state.go("home", {}, { reload: true });
						$modalInstance.dismiss();
					}, function () {
						console.log("log in failed");
					});
			};
		}
	]);