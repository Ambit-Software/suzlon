
//Convert data dd-MM-dd format to yyyy-MM-dd format
var convertDate = function (usDate) {
    var dateParts = usDate.split(/(\d{1,2})-(\d{1,2})-(\d{4})/);
    var dateFormat = dateParts[3] + "-" + dateParts[2] + "-" + dateParts[1];
    if (dateFormat == "undefined-undefined-undefined")
        dateFormat = "undefined";
    return dateFormat;
}