angular.module('fulcrumSeed.global.directives.profile', [])
.directive('visibleWithRoleClaim', [
	'profileSvc', '$rootScope','$state',
	function (profileSvc, $rootScope, $state) {
		return {
			scope: {
				claim: "=visibleWithRoleClaim"
			},
			link: function (scope, element, attrs, claim) {
				element.hide();

				var init = function () {

					var profile = profileSvc.getProfile();

					if (profile.claims !== null) {

						var potential = profile.claims['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

						var foundClaim = _.findWhere(potential, attrs.visibleWithRoleClaim);

						if (foundClaim === undefined) {
							element.hide();
						} else {
							element.show();
						}

					}
				};

				$rootScope.$on('authenticated', function () {
					init();
				});

				$rootScope.$on('$stateChangeSuccess', function () {
					init();
				});

			}
		};
	}
])
.directive('visibleWithoutRoleClaim', [
	'profileSvc', '$rootScope', '$state',
	function (profileSvc, $rootScope, $state) {
		return {
			scope: {
				claim: "=visibleWithoutRoleClaim"
			},
			link: function (scope, element, attrs, claim) {
				element.hide();

				var init = function () {
					var profile = profileSvc.getProfile();

					if (profile.claims !== null) {

						var potential = profile.claims['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

						var foundClaim = _.findWhere(potential, attrs.visibleWithoutRoleClaim);

						if (foundClaim === undefined) {
							element.show();
						} else {
							element.hide();
						}

					}
				};

				if ($localStorage.user == null || $localStorage.user == undefined) {
					init();
				}

				$rootScope.$on('authenticated', function () {
					init();
				});

				$rootScope.$on('$stateChangeSuccess', function () {
					init();
				});

			}
		};
	}
])