export function changeTimeByCurrTimeZone() {
    $("time").each(function (_, e) {
        const dateTimeValue = $(e).attr("datetime");
        if (!dateTimeValue) {
            return;
        }

        const time = moment.utc(dateTimeValue).local();
        $(e).html(time.format("llll"));
        $(e).attr("title", $(e).attr("datetime"));
    });
}

$(changeTimeByCurrTimeZone);
