//
// Copyright (c) 2018 Håkan Edling
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
// 
// http://github.com/piranhacms/piranha.core
// 

if (typeof(piranha)  == 'undefined')
    piranha = {};

Vue.component('media-item', {
    props: ['media'],
    template: '<a href="#"><img :src="media.publicUrl" class="img-thumbnail" data-container=".panel-media" data-toggle="tooltip" data-placement="top" :title="media.filename"></a>'
});

piranha.mediaApp = new Vue({
    el: '#panel-media',
    data: {
        currentFolder: null, 
        items: []
    },
    created: function () {
        this.loadData();
    },
    methods: {
        loadData: function() {
            $.ajax({
                url: piranha.baseUrl + 'manager/api/media/list',
                dataType: 'json',
                success: function (data) {
                    piranha.mediaApp.$data.currentFolder = data.currentFolder;
                    piranha.mediaApp.$data.items = data.items;
                }
            });
        }
    }
});