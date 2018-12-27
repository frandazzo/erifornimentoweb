app.service('httpInitializer', function(){

	$httpProvider.defaults.headers.post = "Content-Type: application/json";
	$httpProvider.defaults.headers.put = "Content-Type: application/json";



})