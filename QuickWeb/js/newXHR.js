;( function ( global )  {
	var lightXHR = ( function () {
		var
			// main ajax
			localXHR,

			// counter
			active,

			// the constructor
			init;

		init = (function( /* global */ ) {
					var xmlHttp;
					// try to instantiate the native XMLHttpRequest object
					try {
						// create an XMLHttpRequest object
						localXHR = new XMLHttpRequest();
					}
					catch(e) {
						// assume IE6 or older
						try {
						  localXHR = new ActiveXObject("Microsoft.XMLHttp");
						}
						catch(e) { }
					}
					 // return the created object or display an error message
					if (!localXHR)
						alert("Error creating the XMLHttpRequest object.");

					/*
					function Ctor( foo ) {
						this.foo = foo;

						return this;
					}

					Ctor.prototype.getFoo = function() {
						return this.foo;
					};

					Ctor.prototype.setFoo = function( val ) {
						return ( this.foo = val );
					};

					// To call constructor's without `new`, you might do this:
					var ctor = function( foo ) {
						return new Ctor( foo );
					};

					// expose our constructor to the parent object
					global.lightXHR.ctor = ctor;

					*/
				})( /* localXHR */ );

		return {
			get : function ( url, callback ) {
					localXHR.open('GET', url, true);
					localXHR.setRequestHeader("Content-Type", "application/json; charset=utf-8");
					localXHR.onreadystatechange = function ( data ) {
                        if ( this.readyState === this.DONE && this.status === 200 ) {
                            callback(this.response);
                        }
                    }
					localXHR.send(null);
				},

			getXML : function ( url, callback )  {
				},
		}
	} )();

	global.lightXHR = lightXHR;
}) ( window )