angular.module('fulcrumSeed.screens.admin.queries', [])
	.config(function($stateProvider) {
		$stateProvider
			.state('admin.queries', {
				url: "/queries",
				templateUrl: "app/screens/admin/queries/queries.html?v=" + APP_VERSION
			});
	})
	.controller('queryController', [
		'$scope', '$state', '$rootScope', 'querySvc', '$modal',
		function($scope, $state, $rootScope, querySvc, $modal) {

			$rootScope.title = "Query List";

			$scope.gridOptions = {
				enableSorting: true,
				enableFiltering: false,
				enableHorizontalScrollbar: 0,
				enableColumnMenus: false,
				columnDefs: [
					{ name: 'Object', field: 'queryObject' },
					{ name: 'Query', field: 'query' },
					{
						name: '',
						field: 'namespace',
						enableSorting: false,
						enableFiltering: false,
						enableColumnMenus: false,
						cellTemplate: "<i class='glyphicon glyphicon-eye-open' ng-click='grid.appScope.runQuery(row.entity)'></i>",
						maxWidth: 90
					},
				],
			};

			var init = function() {

				querySvc.list()
					.then(function(response) {

						$scope.gridOptions.data = response.data;

					}, function(error) {
						// TODO: ??
					});
			};
			init();

			$scope.runQuery = function(query) {
				var runQueryModal = $modal.open({
					templateUrl: 'runQuery.html',
					controller: 'runQueryController',
					size: 'lg',
					resolve: {
						query: function() {
							return query;
						}
					}
				});

				runQueryModal.result.then(function(response) {
					init();
				});
			};

		}
	])
	.controller('runQueryController', [
		'$scope', '$state', 'authSvc', '$rootScope', '$modalInstance',
		'querySvc', 'query',
		function($scope, $state, authSvc, $rootScope, $modalInstance,
		         querySvc, query) {

			$scope.query = query;
			$scope.hasParameters = Object.keys(query.parameters).length > 0;
			
			console.log('query', query);

			var configureForm = function() {
				$scope.form = [
					"*",
					{
						type: "submit",
						title: "Run"
					}
				];

				$scope.formModel = {};

				$scope.schema = query.schema == null ? {} : query.schema;
			};

			var init = function() {
				$scope.gridOptions = {
					enableSorting: true,
					enableFiltering: false,
					enableHorizontalScrollbar: 0,
					enableColumnMenus: false,

				};
			}

			configureForm();
			init();

			$scope.run = function(form, formModel) {
				$scope.$broadcast('schemaFormValidate');

				//console.log('form', form);
				//console.log('formModel', formModel);

				if (form.$valid) {
					// run query
					querySvc.run(query.queryObject + '/' + query.query, formModel)
					.then(function (response) {

						//console.log('response.data.results', response.data.results);

						var resultInArray = response.data.results;

						if (!_.isArray(resultInArray)) {
							var scalar = angular.copy(resultInArray);

							resultInArray = [];

							resultInArray.push(scalar);
						}

						$scope.gridOptions.data = resultInArray;
						$scope.showResults = true;
					}, function (error) {
						// TODO: handle
					});
				}
			};

			$scope.cancel = function() {
				$modalInstance.dismiss();
			};
		}
	]);