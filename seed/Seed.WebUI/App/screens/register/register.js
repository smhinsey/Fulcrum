angular.module('fulcrumSeed.screens.register', [])
		.config(function ($stateProvider) {

			$stateProvider
				.state('register', {
					url: "/register",
					templateUrl: "app/screens/register/register.html?v=" + APP_VERSION
				});

		})
	.controller('registerController', [
		'$scope', '$state', '$rootScope','commandSvc',
		function ($scope, $state, $rootScope, commandSvc) {

			$scope.register = function() {
				
				var cmd = {
					firstName: "",
					lastName: "",
					email: "",
					password: "",
					passwordConfirm: "",
				};

				commandSvc.publish('RegisterAccount', cmd)
					.then(function () {

						console.log('register');

					}, function (error) {
						// TODO: ???
					});

			}

		}
	]);