﻿angular.module('fulcrumSeed.screens.admin.users.accounts', [])
	.config(function($stateProvider) {
		$stateProvider
			.state('admin.users.accounts', {
				url: "/accounts",
				templateUrl: "app/screens/admin/users/accounts/accounts.html?v=" + APP_VERSION
			});
	})
	.controller('userAccountsController', [
		'$scope', '$state', '$rootScope', 'querySvc', '$modal',
		function($scope, $state, $rootScope, querySvc, $modal) {

			$rootScope.title = "User Accounts";

			$scope.gridOptions = {
				enableSorting: true,
				enableFiltering: false,
				enableHorizontalScrollbar: 0,
				enableColumnMenus: false,
				columnDefs: [
					{ name: 'First Name', field: 'firstName' },
					{ name: 'Last Name', field: 'lastName' },
					{ name: 'Email', field: 'email' },
					{ name: 'Registered', field: 'registrationDate', cellFilter: "date:'short'" },
					{ name: 'Last Login', field: 'lastLoginDate', cellFilter: "date:'short'" },
					{
						name: '',
						field: 'id',
						enableSorting: false,
						enableFiltering: false,
						enableColumnMenus: false,
						cellTemplate: "<i class='glyphicon glyphicon-edit' ng-click='grid.appScope.editUser(row.entity)'></i>",
						headerCellTemplate: "<i class='glyphicon glyphicon-plus' ng-click='grid.appScope.newUser()'></i>",
						maxWidth: 90
					},
				],
			};

			var init = function() {
				querySvc.run('UserAccountQueries/ListUsers')
					.then(function(response) {
						var data = response.data.results;

						$scope.gridOptions.data = data;

						$scope.users = response.data.results;
					}, function(error) {
						// TODO: ??
					});
			};
			init();

			$scope.newUser = function() {
				var newUserModal = $modal.open({
					templateUrl: 'newUser.html',
					controller: 'newUserController',
					size: 'md',
				});

				newUserModal.result.then(function(response) {
					init();
				});
			};

			$scope.editUser = function(user) {
				var newUserModal = $modal.open({
					templateUrl: 'editUser.html',
					controller: 'editUserController',
					size: 'lg',
					resolve: {
						user: function() {
							return user;
						}
					}
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
						type: 'text'
					},
					{
						key: 'lastName',
						title: 'Last Name',
						type: 'text'
					},
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
	])
	.controller('editUserController', [
		'$scope', '$state', 'authSvc', '$rootScope', 'authRedirectSvc', '$modalInstance',
		'commandSvc', 'user', 'querySvc', '$q',
		function($scope, $state, authSvc, $rootScope, authRedirectSvc, $modalInstance,
		         commandSvc, user, querySvc, $q) {

			$scope.user = user;

			var configureForm = function() {
				// TODO: centralize form configuration in /global/forms e.g.
				$scope.form = [
					{
						key: 'firstName',
						title: 'First Name',
						type: 'text'
					},
					{
						key: 'lastName',
						title: 'Last Name',
						type: 'text'
					},
					{
						key: 'email',
						title: 'Email',
						type: 'email'
					},
					{
						key: 'groupIds',
						type: 'uiselectmultiple',
						title: 'Group Memberships',
						options: {
							asyncCallback: 'loadGroups',
							map: { valueProperty: 'id', nameProperty: 'groupName' }
						}
					},
					{
						type: "submit",
						title: "Save"
					}
				];

				$scope.formModel = angular.copy(user);
			};
			var init = function() {
				commandSvc.getSchema('EditAccount')
					.then(function(response) {
						$scope.schema = response.data.schema;
					});
			};

			configureForm();
			init();

			$scope.loadGroups = function() {
				return querySvc.run('UserGroupQueries/ListGroups', {}, { returnResultDirectly: true });
			};

			$scope.edit = function(form, formModel) {
				$scope.$broadcast('schemaFormValidate');
				console.log('form', form);
				console.log('formModel', formModel);
				if (form.$valid) {
					commandSvc.publish('EditAccount', formModel)
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