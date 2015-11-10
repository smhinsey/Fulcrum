angular.module('fulcrumSeed.screens.admin.userGroups', [])
		.config(function ($stateProvider) {

			$stateProvider
				.state('admin.userGroups', {
					url: "/userGroups",
					templateUrl: "app/screens/admin/userGroups/userGroups.html?v=" + APP_VERSION
				});

		})
	.controller('userGroupsController', [
		'$scope', '$state', '$rootScope','querySvc','$modal',
		function ($scope, $state, $rootScope, querySvc, $modal) {

			$scope.gridOptions = {
				enableSorting: true,
				enableFiltering: true,
				enableHorizontalScrollbar: 0,
				enableColumnMenus: false,
				columnDefs: [
					{ name: 'Name', field: 'name' },
					{ name: 'Description', field: 'description' },
				],
			};

			var init = function () {

				querySvc.run('UserGroupQueries/ListGroups')
					.then(function (response) {

						var data = response.data.results;

						$scope.gridOptions.data = data;

						$scope.users = response.data.results;
					}, function (error) {
						// TODO: ??
					});
			};
			init();

			$scope.newGroup = function () {
				var newUserModal = $modal.open({
					templateUrl: 'newGroup.html',
					controller: 'newGroupController',
					size: 'md',
				});

				newUserModal.result.then(function (response) {
					init();
				});
			};
		}
	])
	.controller('newGroupController', [
		'$scope', '$state', 'authSvc', '$rootScope', 'authRedirectSvc', '$modalInstance',
		'commandSvc',
		function ($scope, $state, authSvc, $rootScope, authRedirectSvc, $modalInstance,
		         commandSvc) {

			var configureForm = function () {
				// TODO: centralize form configuration in /global/forms e.g.
				$scope.form = [
					{
						key: 'name',
						title: 'Name',
						type: 'text'
					}, ,
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
			var init = function () {
				commandSvc.getSchema('CreateUserClaimGroup')
					.then(function (response) {
						$scope.schema = response.data.schema;
					});
			};

			configureForm();
			init();

			$scope.register = function (form, formModel) {
				$scope.$broadcast('schemaFormValidate');

				if (form.$valid) {
					commandSvc.publish('CreateUserClaimGroup', formModel)
						.then(function (response) {
							$modalInstance.close(response);
						}, function (error) {
							// TODO: wire up errorReporter
						});
				}
			};

			$scope.cancel = function () {
				$modalInstance.dismiss();
			};
		}
	]);