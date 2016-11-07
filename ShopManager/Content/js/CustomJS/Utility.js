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

//Input: C# DateTime - Output: string "21 Tháng 7 1995 at 18:52 PM"
function FormatDateVN_full(date) {
    var milli = date.replace(/\/Date\((-?\d+)\)\//, '$1');
    var d = new Date(parseInt(milli));
    var curD = new Date();
    var hours = d.getHours();
    var monthNames = ["Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6",
     "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"];
    //console.log(curD.getFullYear() + "  " + d.getFullYear());
    //show year if more than 1 year
    if (curD.getFullYear() == d.getFullYear()) {
        return (d.getDate() + " " + monthNames[d.getMonth()]) + " lúc " + hours + ":" + d.getMinutes();
    }
    else return (d.getDate() + " " + monthNames[d.getMonth()]) + " Năm " + d.getFullYear() + " lúc " + hours + ":" + d.getMinutes();
}

//Input: millisecond - Output: string "21 Tháng 7 1995 at 18:52 PM"
function FormatDateVN_fixFBDate(milli) {
    var d = new Date(milli);
    //var newd = new Date();
    var hours = d.getHours();
    var monthNames = ["Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6",
     "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"];
        return (d.getDate() + " " + monthNames[d.getMonth()]) + " lúc " + hours + ":" + d.getMinutes();
}

function GetCurrentDateTime()
{
    var d = new Date();
    var hours = d.getHours();
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12;
    var monthNames = ["Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6",
     "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"];
    return (d.getDate() + " " + monthNames[d.getMonth()]) + " Năm " + d.getFullYear() + " vào lúc " + hours + ":" + d.getMinutes() + ampm;
}

function FormatDateVN_fixFBDate_toSimplified(milli) {
    var d = new Date(milli);
     
    return d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds();
}

function FormatDateTimeVN_Ytr(date) {
    var milli = date.replace(/\/Date\((-?\d+)\)\//, '$1');
    var curDate = new Date();
    var d = new Date(parseInt(milli));
    curDate.setDate(curDate.getDate() - 1);
    if (curDate.getDate = d.getDate && curDate.getMonth == d.getMonth && curDate.getFullYear == d.getFullYear) {
        return "Hôm qua";
    }
    else return d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds();
}
