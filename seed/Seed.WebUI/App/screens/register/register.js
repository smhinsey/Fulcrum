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

			var configureForm = function() {
				$scope.form = [
					"*",
					{
						type: "submit",
						title: "Save"
					}
				];

				$scope.formModel = {};
			};
			var init = function() {
				commandSvc.getSchema('RegisterAccount')
					.then(function(response) {
						$scope.schema = response.data.schema;
					});
			};

			configureForm();
			init();

			$scope.submit = function (form, formModel) {
				$scope.$broadcast('schemaFormValidate');

				if (form.$valid) {
					commandSvc.publish('RegisterAccount', formModel)
						.then(function() {
							console.log('register complete!');
						}, function(error) {
							// TODO: wire up errorReporter
						});
				}
			};
		}
	]);