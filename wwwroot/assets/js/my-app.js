const MyApp = (() => {
   const body = document.querySelector('body');
   const blockUI = new KTBlockUI(body, {
      overflow: 'auto',
      message: `<div class="bg-white rounded px-8 py-3 fs-4 fw-semibold"><span class="spinner-border text-primary me-2"></span> Loading...</div>`,
      zIndex: 99,
   });

   return {
      blockPage: () => {
         return KTApp.showPageLoading();
      },
      unblockPage: () => KTApp.hidePageLoading(),
      initAjaxBlockPage: () => {
         $(document).ajaxStart(function () {
            MyApp.blockPage();
         });

         $(document).ajaxStop(function () {
            MyApp.unblockPage();
         });
      },
      numberFormat: (a, c, d, t) => {
         // a = 47000
         // c = 2
         // d = ,
         // t = .
         // result = 47.000,00
         var n = a,
            c = isNaN((c = Math.abs(c))) ? 2 : c,
            d = d == undefined ? '.' : d,
            t = t == undefined ? ',' : t,
            s = n < 0 ? '-' : '',
            i = parseInt((n = Math.abs(+n || 0).toFixed(c))) + '',
            j = (j = i.length) > 3 ? j % 3 : 0;
         return (
            s +
            (j ? i.substring(0, j) + t : '') +
            i.substring(j).replace(/(\d{3})(?=\d)/g, '$1' + t) +
            (c
               ? d +
                 Math.abs(n - i)
                    .toFixed(c)
                    .slice(2)
               : '')
         );
      },
      alert: (title, message, type = 'info', confirmCallback = null, confirmParameter = null) => {
         var options = {
            title: title,
            text: message,
            icon: type,
            buttonsStyling: false,
            confirmButtonText: 'Ok!',
            customClass: {
               confirmButton: 'btn btn-primary',
            },
         };
         return Swal.fire(options).then((result) => {
            if (result.value) {
               if (confirmCallback) {
                  confirmCallback(confirmParameter);
               }
            }
         });
      },
      confirm: (
         title,
         message,
         type = 'question',
         confirmCallback = null,
         confirmParameter = null,
         cancelCallback = null,
         cancelParameter = null
      ) => {
         var options = {
            title: title,
            text: message,
            icon: type,
            showCancelButton: true,
            confirmButtonText: 'Yes',
         };
         return Swal.fire(options).then((result) => {
            if (result.value) {
               confirmCallback(confirmParameter);
            } else if (result.dismiss === 'cancel') {
               cancelCallback(cancelParameter);
            }
         });
      },
      notify: (title, message, type = 'info') => {
         toastr.options = {
            closeButton: true,
            debug: false,
            newestOnTop: true,
            progressBar: true,
            positionClass: 'toastr-top-right',
            preventDuplicates: true,
            onclick: null,
            showDuration: '300',
            hideDuration: '1000',
            timeOut: '5000',
            extendedTimeOut: '1000',
            showEasing: 'swing',
            hideEasing: 'linear',
            showMethod: 'fadeIn',
            hideMethod: 'fadeOut',
         };

         switch (type) {
            case 'success':
               toastr.success(message, title);
               break;
            case 'warning':
               toastr.warning(message, title);
               break;
            case 'error':
               toastr.error(message, title);
               break;

            default:
               toastr.info(message, title);
               break;
         }
      },
      ajax: (method, url, data = null, callback = null, errorCallback = null) => {
         MyApp.blockPage();
         $.ajax({
            type: method,
            url: url,
            data: data,
            contentType: data instanceof FormData ? false : 'application/x-www-form-urlencoded',
            processData: !(data instanceof FormData),
            dataType: 'JSON',
            success: function (response) {
               if (callback) {
                  callback(response);
               }
            },
            error: function (response, status, error) {
               const json = response.responseJSON;
               if (errorCallback) {
                  return errorCallback(json);
               }
               MyApp.alert('Error', json.message, 'error');
            },
         }).always(function () {
            MyApp.unblockPage();
         });
      },
      ajaxGet: (url, data = null, callback = null, errorCallback = null) => {
         MyApp.ajax('GET', url, data, callback, errorCallback);
      },
      ajaxPost: (url, data = null, callback = null, errorCallback = null) => {
         MyApp.ajax('POST', url, data, callback, errorCallback);
      },
      ajaxPut: (url, data = null, callback = null, errorCallback = null) => {
         MyApp.ajax('PUT', url, data, callback, errorCallback);
      },
      ajaxDelete: (url, data = null, callback = null, errorCallback = null) => {
         MyApp.ajax('DELETE', url, data, callback, errorCallback);
      },
   };
})();

window.MyApp = MyApp;
