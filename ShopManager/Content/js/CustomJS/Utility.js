//Format VN date
//Input: C# DateTime - Output: string "21/07/1995 18:52:49"
function FormatDateTimeVN(date) {
    var milli = date.replace(/\/Date\((-?\d+)\)\//, '$1');
    var d = new Date(parseInt(milli));
    return d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds();
}

//Input: C# DateTime - Output: string "21/07/1995"
function FormatDateVN(date) {
    console.log(date);
    var milli = date.replace(/\/Date\((-?\d+)\)\//, '$1');
    var d = new Date(parseInt(milli));
    return d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear();
}

//Input: C# DateTime - Output: string "21/07 18:52"
function FormatChatDateTimeVN(date) {
    var milli = date.replace(/\/Date\((-?\d+)\)\//, '$1');
    var d = new Date(parseInt(milli));
    return d.getDate() + "/" + (d.getMonth() + 1) + " " + d.getHours() + ":" + d.getMinutes();
}

//For realtime message
//Input: millisecond - Output: string "21/07 18:52"
function FormatChatDateRealtimeVN(milli) {
    var d = new Date(milli);
    return d.getDate() + "/" + (d.getMonth() + 1) + " " + d.getHours() + ":" + d.getMinutes();
}

// ... minutes ago
function GetDateAgo(d) {
    var milli = d.replace(/\/Date\((-?\d+)\)\//, '$1');
    var date = new Date(parseInt(milli));

    var seconds = Math.floor((new Date() - date) / 1000);

    var interval = Math.floor(seconds / 31536000);
    if (interval >= 1) {
        return interval + " năm trước";
    }
    interval = Math.floor(seconds / 2592000);
    if (interval >= 1) {
        return interval + " tháng trước";
    }
    interval = Math.floor(seconds / 86400);
    if (interval >= 1) {
        return interval + " ngày trước";
    }
    interval = Math.floor(seconds / 3600);
    if (interval >= 1) {
        return interval + " giờ trước";
    }
    interval = Math.floor(seconds / 60);
    if (interval >= 1) {
        return interval + " phút trước";
    }
    return Math.floor(seconds) + " giây trước";
}