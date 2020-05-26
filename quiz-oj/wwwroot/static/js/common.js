var host = "http://127.0.0.1:7070"

var checkIfLoggedIn = function (loggedInFn, nonFn) {
    $.get({
        url: host + "/api/auth/checkIfLoggedIn",
        withCredentials: true,
        success: function (data) {
            if(data.code === 0 && data.data != null){
                if(loggedInFn){
                    loggedInFn(data.data);
                }
            }else{
                if(nonFn){
                    nonFn();
                }
            }
        }
    })
}

var checkData = function (data) {
    if(data.code === -1){
        alert("Error: " + data.message);
        return false;
    }
    return true;
}

var clearCookie = function (){
    var keys=document.cookie.match(/[^ =;]+(?=\=)/g);
    if (keys) {
        for (var i =  keys.length; i--;)
            document.cookie = keys[i] + '=0;path=/;expires=' + new Date(0).toUTCString();// 清除当前域名路径的有限日期
        document.cookie = keys[i] + '=0;path=/;domain=' + document.domain + ';expires=' + new Date(0).toUTCString();// Domain Name域名 清除当前域名的
        document.cookie = keys[i] + '=0;path=/;domain=baidu.com;expires=' + new Date(0).toUTCString();// 清除一级域名下的或指定的
    }
}

String.prototype.getQuery = function(name) {
    var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
    var r = this.substr(this.indexOf("\?")+1).match(reg);
    if (r!=null) return unescape(r[2]); return null;
}