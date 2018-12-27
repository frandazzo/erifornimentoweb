app.service('AuthenticationService', function($rootScope, $location, 
													$http, serverBaseUrl, $q,
													$timeout){

	var self = this;

	//proprietà che indica se un utente è stato autenticato oppure no
	this.authenticated = false;
	//lista dei path dell'app non protetti
	this._unprotectedPath = [
		'/login'
	];

	this._clearClientData = function(){
		localStorage.setItem('currentUser', '');
		$http.defaults.headers.common['authorization'] = null;
		self.authenticated = false;
		self.currentUser = {};
	}


	this._setClientData = function(currentUser){
		localStorage.setItem('currentUser', JSON.stringify(currentUser));
		self.authenticated = true;
		self.currentUser = currentUser;
		$http.defaults.headers.common['authorization'] = 'Bearer ' + currentUser.token;
	}

	this.login = function(username, password){
		//avvia il login al server...
		//se tutto è ok salva i dati dell'utente loggato nel
		//localstorage e imposta il token di autenticazione par ogni chiamata
		//ajax.....
		var deferred = $q.defer();

		var credentials = {username:username,password: password};


		var promise = $http.post(serverBaseUrl + 'api/authenticate', credentials);
		promise.then(
			function(response){

				
					var currentUser = response.data;
					//currentUser.token = response.headers()['authorization'];		
					//imposto i dati client
			 		self._setClientData(currentUser);
			 		deferred.resolve(currentUser);
				
				

			}, function(error){
				self._clearClientData();
		 		deferred.reject(error);
			});


		// $timeout(function() {

		// 	if (username == 'ciccio' && password == 'ciccio'){

		// 		var currentUser = {
		// 		    _id: "599ff28876f5030011aae958",
		// 		    name: "ATOM",
		// 		    mail: "atom@atom.it",
		// 		    role: "ADMIN",
		// 		    __v: 3,
		// 		    active: true,
		// 		    contracts: []
		// 		}

		// 		currentUser.token = 'dhfuherifuehfuiwehfuipef-wefewifjwe0fowjef';				
		// 		//imposto i dati client
		// 		self._setClientData(currentUser);

		// 		//risolvo la promise
		// 		deferred.resolve(currentUser);

		// 	}else{

		// 		self._clearClientData();
		// 		deferred.reject('error');

		// 	}
			
		// }, 500);

		return deferred.promise;



	}


	this.logout = function(){

		//eseguo un logout dal server
		var deferred = $q.defer();


		var promise = $http.post(serverBaseUrl + 'api/logout');
		promise.then(
			function(response){

				localStorage.setItem('currentUser', '');
				$http.defaults.headers.common['authorization'] = null;
				self.authenticated = false;
				self.currentUser = {};
				deferred.resolve();

			}, function(error){
				
		 		deferred.reject(error);
			});

		// $timeout(function() {
		// 	//rimuovo tutte le informaizoni dal client
		// 	localStorage.setItem('currentUser', '');
		// 	$http.defaults.headers.common['authorization'] = null;
		// 	self.authenticated = false;
		// 	self.currentUser = {};
		// 	deferred.resolve();

		// }, 500);

		return deferred.promise;
		
	}


	//la funzione init inizializza il servizio di autenticazione
	//nel run dell'applicazione
	this.init = function(){

		//l'inizializzazione prevede la verifica del nuovo url richiesto nell'applicazione
		//se questo url è relativo ad un path protetto allora verifico l'autenticazione
		//e se non autenticato redirigo alla pagina di login


		//la verifica del nuovo url avviene nel listener dell'evento $routeChangeStart dell'oggetto 
		//$route. tale evento è innescato dalla change dell'oggetto $location

		//il listener lo aggancio al rootscope
		$rootScope.$on('$routeChangeStart', function(event, next, current){

				//next e current sono entrabi oggetti $route e rappresentano
				// rispettivamente le info di route di arrivo e di partenza

				//recupero il path di destinazione
				var destPath = $location.path();
				//se il path di destinazione non è compreso nei path non prottetti
				//allora verifico l'autenticazione
				if (self._unprotectedPath.indexOf(destPath) === -1){

					//la verifica dell'autenticazione implica che 
					//ci sia nel local storage un oggetto currentUser impostato in fase di login
					//se non cè vuol dire che non ho autenticazione
					var currentUser = localStorage.getItem('currentUser');
					if (!currentUser){
						//non ci sono dati sull'utente loggato
						//redirigo
						$location.path('/login');
					}else{

						//se ci sono i dati dell'utente loggato allora
						//recupero il token di autenticazione e lo inserisco
						//tra i parametri di dell'header delle chiamate http
						var currenUserObj = JSON.parse(currentUser);
						var token = currenUserObj.token;
						$http.defaults.headers.common['authorization'] = 'Bearer ' + token;
						//imposto il flag di autenticato a true
						self.authenticated = true;
						self.currentUser = currenUserObj;

					}
				}



		})


	}


});