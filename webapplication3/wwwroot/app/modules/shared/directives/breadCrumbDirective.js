app.directive('breadCrumb', function(AuthenticationService) {

  return {
  	transclude: true,
    restrict: 'EA',
    templateUrl: '../../../../../app/modules/shared/views/breadCrumbView.html'
  };
});