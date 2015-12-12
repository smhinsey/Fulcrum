angular.module('fulcrumSeed.screens.admin.users.sessions', [])
		.config(function ($stateProvider) {

			$stateProvider
				.state('admin.users.sessions', {
					url: "/sessions",
					templateUrl: "app/screens/admin/users/sessions/sessions.html?v=" + APP_VERSION
				});

		})
	.controller('userSessionsController', [
		'$scope', '$state', '$rootScope','querySvc',
		function ($scope, $state, $rootScope, querySvc) {

			$rootScope.title = "User Sessions";

			$scope.gridOptions = {
				enableSorting: true,
				enableFiltering: false,
				enableHorizontalScrollbar: 0,
				enableColumnMenus: false,
				columnDefs: [
					{ name: 'Username', field: 'username' },
					{ name: 'Logged In', field: 'name' },
					{ name: 'Last Seen', field: 'name' },
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

			var init = function () {
				querySvc.run('UserSessionQueries/GetAll')
					.then(function (response) {

						var data = response.data.results;

						angular.forEach(data, function (s) {
							s.code = JSON.parse(s.rawJson);

							var matchedClaim = _.findWhere(s.code.AccessToken.Claims, { Type: 'preferred_username' });

							console.log('matchedClaim', matchedClaim);

							s.username = matchedClaim.Value;
						});

						console.log('data', data);

						$scope.gridOptions.data = data;

						$scope.users = response.data.results;
					}, function (error) {
						// TODO: ??
					});
			};
			init();
		}
	]);