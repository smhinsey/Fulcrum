angular.module('fulcrumSeed.screens.admin.queries', [])
	.config(function ($stateProvider) {
		$stateProvider
			.state('admin.queries', {
				url: "/queries",
				templateUrl: "app/screens/admin/queries/queries.html?v=" + APP_VERSION
			});
	})