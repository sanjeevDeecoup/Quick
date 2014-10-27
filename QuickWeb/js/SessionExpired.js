function initSession() {
    sess_lastActivity = new Date();
    sessSetInterval();
    $(document).bind('keypress.session', function (ed, e) {
        sessKeyPressed(ed, e);
    });
}
function sessSetInterval() {
    sess_intervalID = setInterval('sessInterval()', sess_pollInterval);
}
function sessClearInterval() {
    clearInterval(sess_intervalID);
}
function sessKeyPressed(ed, e) {
    sess_lastActivity = new Date();
}
function sessLogOut() {
    window.location.href = '../Login.aspx?option=backup';
}
function sessInterval() {
    var now = new Date();
    //get milliseconds of differneces
    var diff = now - sess_lastActivity;
    //get minutes between differences
    var diffMins = (diff / 1000 / 60);
    if (diffMins >= sess_warningMinutes) {
        //wran before expiring
        //stop the timer
        sessClearInterval();

        //promt for attention
        //                var active = confirm('Your session will expire');
        //
        //                if (active == true) {
        now = new Date();
        diff = now - sess_lastActivity;
        diffMins = (diff / 1000 / 60);
        if (diffMins > sess_expirationMinutes) {
            sessLogOut();
            //                    }
            //                    else {
            //                        initSession();
            //                        sessSetInterval();
            //                        sess_lastActivity = new Date();
            //                    }
        }
        else {
            sessLogOut();
        }
    }
}     