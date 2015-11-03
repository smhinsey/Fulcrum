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
					{
						key: 'firstName',
						title: 'First Name',
						type: 'text'
					},
					,
					{
						key: 'lastName',
						title: 'Last Name',
						type: 'text'
					},
					,
					{
						key: 'email',
						title: 'Email',
						type: 'email'
					},
					{
						key: 'password',
						title: 'Password',
						type: 'password'
					},
					{
						key: 'passwordConfirm',
						title: 'Confirm Password',
						type: 'password'
					},
					{
						type: "submit",
						title: "Register"
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

			$scope.register = function (form, formModel) {
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