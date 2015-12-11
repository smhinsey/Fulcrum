angular.module('fulcrumSeed.screens.admin.commands', [])
	.config(function($stateProvider) {
		$stateProvider
			.state('admin.commands', {
				url: "/commands",
				templateUrl: "app/screens/admin/commands/commands.html?v=" + APP_VERSION
			});

		$stateProvider
			.state('admin.commandRegistry', {
				url: "/commands/publication-registry",
				templateUrl: "app/screens/admin/commands/registry.html?v=" + APP_VERSION
			});
	})
	.controller('commandController', [
		'$scope', '$state', '$rootScope', 'commandSvc', '$modal',
		function($scope, $state, $rootScope, commandSvc, $modal) {

			$rootScope.title = "Command List";

			$scope.gridOptions = {
				enableSorting: true,
				enableFiltering: false,
				enableHorizontalScrollbar: 0,
				enableColumnMenus: false,
				columnDefs: [
					{ name: 'Namespace', field: 'namespace' },
					{ name: 'Name', field: 'name' },
					{
						name: '',
						field: 'namespace',
						enableSorting: false,
						enableFiltering: false,
						enableColumnMenus: false,
						cellTemplate: "<i class='glyphicon glyphicon-eye-open' ng-click='grid.appScope.publish(row.entity)'></i>",
						maxWidth: 90
					},
				],
			};

			var init = function() {
				commandSvc.list()
					.then(function(response) {
						$scope.gridOptions.data = response.data;
					}, function(error) {
						// TODO: ??
					});
			};
			init();

			$scope.publish = function(command) {
				var publishCmdModal = $modal.open({
					templateUrl: 'publish.html',
					controller: 'publishCommandController',
					size: 'lg',
					resolve: {
						command: function() {
							return command;
						}
					}
				});

				publishCmdModal.result.then(function(response) {
					init();
				});
			};

		}
	])
	.controller('publishCommandController', [
		'$scope', '$state', 'authSvc', '$rootScope', '$modalInstance',
		'commandSvc', 'command', 'querySvc',
		function($scope, $state, authSvc, $rootScope, $modalInstance,
		         commandSvc, command, querySvc) {

			$scope.command = command;
			$scope.showResults = false;

			var configureForm = function() {
				$scope.form = [
					"*",
					{
						type: "submit",
						title: "Publish"
					}
				];

				$scope.formModel = {};

				$scope.schema = command.schema == null ? {} : command.schema;
			};

			var init = function() {
				$scope.linkGrid = {
					enableSorting: true,
					enableFiltering: false,
					enableHorizontalScrollbar: 0,
					enableColumnMenus: false,
				};
				$scope.queryGrid = {
					enableSorting: true,
					enableFiltering: false,
					enableHorizontalScrollbar: 0,
					enableColumnMenus: false,
				};
			};
			configureForm();
			init();

			$scope.publish = function(form, formModel) {
				$scope.$broadcast('schemaFormValidate');

				if (form.$valid) {
					commandSvc.publish(command.name, formModel)
						.then(function(response) {

							$scope.pubRecord = response;
							$scope.linkGrid.data = response.links;
							$scope.queryGrid.data = response.queryReferences;

							$scope.showResults = true;
						}, function(error) {
							// TODO: handle
						});
				}
			};

			$scope.cancel = function() {
				$modalInstance.dismiss();
			};

			$scope.runQuery = function(query) {
				querySvc.run(q.queryName, q.parameters)
					.then(function (response) {
						console.log('response', response);


					}, function (error) {
						// TODO: handle error
					});
			}
		}
	])
	.controller('commandRegistryController', [
		'$scope', '$state', '$rootScope', 'commandSvc', '$modal',
		function ($scope, $state, $rootScope, commandSvc, $modal) {

			$rootScope.title = "Command Registry";

			$scope.gridOptions = {
				enableSorting: true,
				enableFiltering: false,
				enableHorizontalScrollbar: 0,
				enableColumnMenus: false,
				columnDefs: [
					{ name: 'Created', field: 'created' },
					{ name: 'Updated', field: 'updated' },
					{ name: 'Status', field: 'status' },
					{ name: 'Command Name', field: 'commandName' },
					{
						name: '',
						field: 'id',
						enableSorting: false,
						enableFiltering: false,
						enableColumnMenus: false,
						cellTemplate: "<i class='glyphicon glyphicon-eye-open' ng-click='grid.appScope.details(row.entity)'></i>",
						maxWidth: 90
					},
				],
			};

			var init = function () {
				commandSvc.showRegistry()
					.then(function (response) {
						$scope.gridOptions.data = response.data;
					}, function (error) {
						// TODO: ??
					});
			};
			init();

			$scope.details = function (record) {
				var detailsModal = $modal.open({
					templateUrl: 'recordDetails.html',
					controller: 'publicationRecordDetailsController',
					size: 'lg',
					resolve: {
						record: function () {
							return record;
						}
					}
				});

				detailsModal.result.then(function (response) {
					init();
				});
			};
		}

	])
	.controller('publicationRecordDetailsController', [
		'$scope', '$state', 'authSvc', '$rootScope', '$modalInstance',
		'commandSvc', 'record','querySvc',
		function ($scope, $state, authSvc, $rootScope, $modalInstance,
		         commandSvc, record, querySvc) {

			$scope.record = record;

			//console.log('record', record);

			var init = function () {
				$scope.resultGrid = {
					enableSorting: true,
					enableFiltering: false,
					enableHorizontalScrollbar: 0,
					enableColumnMenus: false,

				};
			}

			var isArray = function(myArray) {
				return myArray.constructor.toString().indexOf("Array") > -1;
			}

			init();

			$scope.cancel = function () {
				$modalInstance.dismiss();
			};

			$scope.runQuery = function (q) {
				querySvc.run(q.queryName, q.parameters, { skipNameWildcard: true })
					.then(function (response) {
						console.log('response', response);

						var resultInArray = response.data.results;

						if (!isArray(resultInArray)) {
							var scalar = angular.copy(resultInArray);

							resultInArray = [];

							resultInArray.push(scalar);
						}

						$scope.resultGrid.data = resultInArray;
						$scope.showResults = true;
					}, function (error) {
						// TODO: handle error
					});
			}
		}
	])
;