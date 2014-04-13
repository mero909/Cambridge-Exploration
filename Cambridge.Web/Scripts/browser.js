function msieversion() {
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");

    if (msie > 0)      // If Internet Explorer, return version number
        alert(parseInt(ua.substring(msie + 5, ua.indexOf(".", msie))));

    return false;
}