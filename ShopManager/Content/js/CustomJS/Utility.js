//Format VN date
//Input: C# DateTime - Output: string "21/07/1995 18:52:49"
function FormatDateTimeVN(date) {
    var milli = date.replace(/\/Date\((-?\d+)\)\//, '$1');
    var d = new Date(parseInt(milli));
    return d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds();
}

//Input: C# DateTime - Output: string "21/07/1995"
function FormatDateVN(date) {
    var milli = date.replace(/\/Date\((-?\d+)\)\//, '$1');
    var d = new Date(parseInt(milli));
    return d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear();
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

