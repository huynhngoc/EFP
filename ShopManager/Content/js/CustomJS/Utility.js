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

    var now = new Date();
    var day = "";
    if (d.getDate() == now.getDate()) {
        day = "Hôm nay";
    }
    else if (d.getDate() == now.getDate() - 1) {
        day = "Hôm qua";
    }
    else {
        day = d.getDate() + "/" + (d.getMonth() + 1);
        if (now.getFullYear() != d.getFullYear()) {
            day += "/" + d.getFullYear().toString().substr(2, 2);
        }
    }

    var hours = d.getHours();
    var minutes = d.getMinutes();
    var ampm = hours >= 12 ? 'pm' : 'am';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ampm;

    return day + " " + strTime;
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

//For realtime message
//Input: millisecond - Output: string "21/07 18:52"
function FormatChatDateRealtimeVN(milli) {
    var d = new Date(milli);

    var now = new Date();
    var day = ""
    if (d.getDate() == now.getDate()) {
        day = "Hôm nay";
    }
    else if (d.getDate() == now.getDate() - 1) {
        day = "Hôm qua";
    }
    else {
        day = d.getDate() + "/" + (d.getMonth()+1);
        if (now.getFullYear() != d.getFullYear()) {
            day += "/" + d.getFullYear().toString().substr(2, 2);
        }
    }

    var hours = d.getHours();
    var minutes = d.getMinutes();
    var ampm = hours >= 12 ? 'pm' : 'am';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ampm;

    return day + " " + strTime;
}

// ... minutes ago - Input: millisecond
function GetDateAgoRealTime(milli) {
    var date = new Date(milli);

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

// Output: string "Hôm nay 12:05am"