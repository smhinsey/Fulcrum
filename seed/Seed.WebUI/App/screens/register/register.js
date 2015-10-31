angular.module('fulcrumSeed.screens.register', [])
		.config(function ($stateProvider) {

			$stateProvider
				.state('register', {
					url: "/register",
					templateUrl: "app/screens/register/register.html?v=" + APP_VERSION
				});

		})
	.controller('registerController', [
		'$scope', '$state', '$rootScope',
		function ($scope, $state, $rootScope) {

		}
	]);