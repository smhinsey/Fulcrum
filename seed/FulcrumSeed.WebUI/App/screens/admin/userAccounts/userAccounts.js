angular.module('fulcrumSeed.screens.admin.userAccounts', [])
	.config(function($stateProvider) {
		$stateProvider
			.state('admin.userAccounts', {
				url: "/userAccounts",
				templateUrl: "app/screens/admin/userAccounts/userAccounts.html?v=" + APP_VERSION
			});
	})
	.controller('userAccountsController', [
		'$scope', '$state', '$rootScope', 'querySvc', '$modal',
		function($scope, $state, $rootScope, querySvc, $modal) {

			$scope.gridOptions = {
				enableSorting: true,
				enableFiltering: true,
				enableHorizontalScrollbar: 0,
				enableColumnMenus: false,
				columnDefs: [
					{ name: 'First Name', field: 'firstName' },
					{ name: 'Last Name', field: 'lastName' },
					{ name: 'Email', field: 'email' },
					{
						name: '',
						field: 'id',
						enableSorting: false,
						enableFiltering: false,
						enableColumnMenus: false,
						cellTemplate: '<span></span>',
						headerCellTemplate: "<button class='gridBtn btn btn-primary' ng-click='grid.appScope.newUser()'>New</button>",
						maxWidth: 90
					},
				],
			};

			var init = function() {

				querySvc.run('UserAccountQueries/ListUsers')
					.then(function(response) {

						var data = response.data.results;

						console.log('data', data);

						$scope.gridOptions.data = data;

						$scope.users = response.data.results;
					}, function(error) {
						// TODO: ??
					});
			};
			init();

			$scope.newUser = function() {
				console.log('yo');
				var newUserModal = $modal.open({
					templateUrl: 'newUser.html',
					controller: 'newUserController',
					size: 'md',
				});

				newUserModal.result.then(function(response) {
					init();
				});
			};
		}
	])
	.controller('newUserController', [
		'$scope', '$state', 'authSvc', '$rootScope', 'authRedirectSvc', '$modalInstance',
		'commandSvc',
		function($scope, $state, authSvc, $rootScope, authRedirectSvc, $modalInstance,
		         commandSvc) {

			var configureForm = function() {
				// TODO: centralize form configuration in /global/forms e.g.
				$scope.form = [
					{
						key: 'firstName',
						title: 'First Name',
						ype: 'text'
					},,
					{
						key: 'lastName',
						title: 'Last Name',
						type: 'text'
					},,
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

			$scope.register = function(form, formModel) {
				$scope.$broadcast('schemaFormValidate');

				if (form.$valid) {
					commandSvc.publish('RegisterAccount', formModel)
						.then(function(response) {
							$modalInstance.close(response);
						}, function(error) {
							// TODO: wire up errorReporter
						});
				}
			};

			$scope.cancel = function() {
				$modalInstance.dismiss();
			};
		}
	]);