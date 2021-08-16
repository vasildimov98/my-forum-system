$("#ajaxSelect2").select2({
    placeholder: "Search category",
    theme: "bootstrap4",
    allowClear: true,
    width: '100%',
    ajax: {
        url: "/api/categories/search",
        contentType: "application/json; charset=utf-8",
        data: function (params) {
            var query =
            {
                term: params.term,
            };
            return query;
        },
        processResults: function (result) {
            return {
                results: $.map(result, function (item) {
                    return {
                        id: item.id,
                        text: item.name
                    };
                }),
            };
        }
    }
});
