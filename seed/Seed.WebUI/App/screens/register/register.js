angular.module('fulcrumSeed.screens.register', [])
	.config(function($stateProvider) {

		$stateProvider
			.state('register', {
				url: "/register",
				templateUrl: "app/screens/register/register.html?v=" + APP_VERSION
			});

	})
	.controller('registerController', [
		'$scope', '$state', '$rootScope', 'commandSvc',
		function($scope, $state, $rootScope, commandSvc) {

			$scope.form = [
				"*",
				{
					type: "submit",
					title: "Save"
				}
			];

			commandSvc.getSchema('RegisterAccount')
				.then(function(response) {
					$scope.schema = response.data.schema;

					console.log('schema', $scope.schema);
				});


			$scope.register = function() {

				// TODO: replace this with angular-schema-form/tv4.js driven system

				var cmd = {
					firstName: $scope.firstName,
					lastName: $scope.lastName,
					email: $scope.email,
					password: $scope.password,
					passwordConfirm: $scope.password,
				};

				commandSvc.publish('RegisterAccount', cmd)
					.then(function() {

						console.log('register');

					}, function(error) {
						// TODO: ???
					});

			};
		}
	]);