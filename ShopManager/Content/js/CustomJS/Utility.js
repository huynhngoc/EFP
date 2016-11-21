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

//Input: C# DateTime - Output: string "21 Tháng 7 1995 at 18:52 PM"
function FormatDateVN_full(date) {
    var milli = date.replace(/\/Date\((-?\d+)\)\//, '$1');
    var d = new Date(parseInt(milli));
    var curD = new Date();
    var monthNames = ["Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6",
     "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"];
    //console.log(curD.getFullYear() + "  " + d.getFullYear());
    //show year if more than 1 year
    if (curD.getFullYear() == d.getFullYear()) {
        return (d.getDate() + " " + monthNames[d.getMonth()]) + " lúc " + d.getHours() + ":" + d.getMinutes();
    }
    else return (d.getDate() + " " + monthNames[d.getMonth()]) + " Năm " + d.getFullYear() + " lúc " + d.getHours() + ":" + d.getMinutes();
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

function GetCurrentDateTime() {
    var d = new Date();
    var hours = d.getHours();
    var monthNames = ["Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6",
     "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"];
    return (d.getDate() + " " + monthNames[d.getMonth()]) + " lúc " + hours + ":" + d.getMinutes();
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
//from page to fb 
function TextReplace(text) {
    if (text && text != "") {
        return text = text.replace(/\n/g, "<br>").replace(/\r/g, "\\\\r").replace(/\t/g, "\\\\t")
    }
    else {
        return "";
    }
}
//from fb to page
function TextReplacedToChat(text) {
    if (text && text != "") {
        return text = text.replace("<br>", /\n/g).replace("\\\\r", /\r/g).replace("\\\\t", /\t/g).replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/\n/g, "<br>");
    }
    else {
        return "";
    }
}

function ShowMoreTextFunc(text) {
    var match = /\r|\n/.exec(text);
    if (match) {
        // Found line breaks;
        var firstLine = text.split('\n')[0];
        return firstLine;
    }
    else if (text.indexOf('<br>') != -1) return text.split('<br>')[0];
    else return text;
}