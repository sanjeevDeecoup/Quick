; (function (global) {
    var TPL = (function () {
        var
        // the action block
			ActionBlock,

        // the buffer block
			BufferBlock,

        // the transform block
			TransformBlock;

        // set it to action
        ActionBlock = (function () {
            // for internal storage
            var
				_type,

				init,

				localAction,

				LinkTo;

            _type = 'ActionBlock';

            init: (function () {
            })( /* fn */);

            /*
            LinkTo : function ( fn ) {
            };*/

            return {
                Type: _type,

                set: function (fn) {
                    this.localAction = fn;
                }

                /* LinkTo : LinkTo */
            };
        })();

        // set the buffer block
        BufferBlock = (function () {
            // for internal storage
            var
				_type,

				init,

				localBuffer,

				capacity,

				count,

				connectedAction,

				LinkTo;

            _type = 'BufferBlock';

            init: (function () {
            })( /* fn */);

            LinkTo: (function () {
                return {
                    link: function (fn) {
                        // check if fn exposes a property _type, and see if its of action block
                        if (!fn.Type)
                            throw ('Invaid Type');
                    }
                }
            })( /* fn */);

            return {
                Type: _type,

                start: function (fn, times) {
                    this.localBuffer = fn;
                    this.capacity = times;
                    this.count = 0;
                },

                set: function (fn) {
                    this.connectedAction = fn;
                },

                exec: function (times) {
                    var time = times || 1;

                    if (!this.localBuffer)
                        throw ('No function set');

                    if (typeof this.localBuffer !== 'function')
                        throw ('Invaid Type');

                    while (time--) {
                        debugger;
                        this.localBuffer();
                        this.count++;

                        if (this.count == this.capacity) {
                            this.connectedAction();
                        }
                    }
                },

                LinkTo: LinkTo
            };
        })();

        // set to transform block
        /* TransformBlock = (

        )(); */

        return {
            ActionBlock: ActionBlock,

            BufferBlock: BufferBlock,

            TransformBlock: 'TransformBlock'
        }
    })();

    global.TPL = TPL;
})(window);