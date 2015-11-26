angular.module('fulcrumSeed.screens.admin.users.roles', [])
	.config(function($stateProvider) {

		$stateProvider
			.state('admin.users.roles', {
				url: "/roles",
				templateUrl: "app/screens/admin/users/roles/roles.html?v=" + APP_VERSION
			});

	})
	.controller('userRolesController', [
		'$scope', '$state', '$rootScope', 'querySvc', '$modal',
		function($scope, $state, $rootScope, querySvc, $modal) {

			$rootScope.title = "User Roles";

			$scope.gridOptions = {
				enableSorting: true,
				enableFiltering: false,
				enableHorizontalScrollbar: 0,
				enableColumnMenus: false,
				columnDefs: [
					{ name: 'Name', field: 'name' },
					{ name: 'Description', field: 'description' },
					{
						name: '',
						field: 'roleId',
						enableSorting: false,
						enableFiltering: false,
						enableColumnMenus: false,
						cellTemplate: "<i class='glyphicon glyphicon-edit' ng-click='grid.appScope.editRole(row.entity)'></i>",
						maxWidth: 90
					},
				],
			};

			var init = function() {

				querySvc.run('UserRoleQueries/ListRoles')
					.then(function(response) {

						var data = response.data.results;

						$scope.gridOptions.data = data;

						$scope.users = response.data.results;
					}, function(error) {
						// TODO: ??
					});
			};
			init();

			$scope.newRole = function() {
				var newUserModal = $modal.open({
					templateUrl: 'newRole.html',
					controller: 'newRoleController',
					size: 'md',
				});

				newUserModal.result.then(function(response) {
					init();
				});
			};

			$scope.editRole = function(role) {
				var newUserModal = $modal.open({
					templateUrl: 'editRole.html',
					controller: 'editRoleController',
					size: 'md',
					resolve: {
						role: function () {
							return role;
						}
					}
				});

				newUserModal.result.then(function(response) {
					init();
				});
			};
		}
	])
	.controller('newRoleController', [
		'$scope', '$state', 'authSvc', '$rootScope', 'authRedirectSvc', '$modalInstance',
		'commandSvc',
		function($scope, $state, authSvc, $rootScope, authRedirectSvc, $modalInstance,
		         commandSvc) {

			var configureForm = function() {
				// TODO: centralize form configuration in /global/forms e.g.
				$scope.form = [
					{
						key: 'name',
						title: 'Name',
						type: 'text'
					},,
					{
						key: 'description',
						title: 'Description',
						type: 'text'
					},
					{
						type: "submit",
						title: "Create"
					}
				];

				$scope.formModel = {};
			};
			var init = function() {
				commandSvc.getSchema('CreateUserClaimGroup')
					.then(function(response) {
						$scope.schema = response.data.schema;
					});
			};

			configureForm();
			init();

			$scope.register = function(form, formModel) {
				$scope.$broadcast('schemaFormValidate');

				if (form.$valid) {
					commandSvc.publish('CreateUserRole', formModel)
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
	])
	.controller('editRoleController', [
		'$scope', '$state', 'authSvc', '$rootScope', 'authRedirectSvc', '$modalInstance',
		'commandSvc','role',
		function($scope, $state, authSvc, $rootScope, authRedirectSvc, $modalInstance,
		         commandSvc, role) {

			console.log('role', role);

			var configureForm = function() {
				// TODO: centralize form configuration in /global/forms e.g.
				$scope.form = [
					{
						key: 'description',
						title: 'Description',
						type: 'textarea'
					},
					{
						type: "submit",
						title: "Save"
					}
				];

				$scope.formModel = angular.copy(role);
			};
			var init = function() {
				commandSvc.getSchema('EditUserRole')
					.then(function(response) {
						$scope.schema = response.data.schema;
					});
			};

			configureForm();
			init();

			$scope.save = function(form, formModel) {
				$scope.$broadcast('schemaFormValidate');

				if (form.$valid) {
					commandSvc.publish('EditUserRole', formModel)
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