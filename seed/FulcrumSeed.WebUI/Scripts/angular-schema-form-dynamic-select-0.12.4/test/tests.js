/* jshint expr: true */
chai.should();

describe('Schema form', function () {

    describe('directive', function () {
        beforeEach(module('templates'));
        beforeEach(module('schemaForm'));
        beforeEach(module('mgcrea.ngStrap'));
        beforeEach(
            // We don't need no sanitation. We don't need no thought control.
            // (nicklasb: I don't totally understand this Floyd reference, so I better leave it in there :-)
            module(function ($sceProvider) {
                $sceProvider.enabled(false);
            })
        );

        // Populate a given scope with test data.
        var assignToScope = function (scope, http) {

            scope.callBackSD = function (options) {
                return [
                    {value: 'value1', name: 'text1'},
                    {value: 'value2', name: 'text2'},
                    {value: 'value3', name: 'Select dynamic!'}
                ];
                // Note: Options is a reference to the original instance, if you change a value,
                // that change will persist when you use this form instance again.
            };

            scope.callBackMSD = function (options) {
                return [
                    {value: 'value1', name: 'text1'},
                    {value: 'value2', name: 'text2'},
                    {value: 'value3', name: 'Multiple select dynamic!'}
                ];
                // Note: Options is a reference to the original instance, if you change a value,
                // that change will persist when you use this form instance again.
            };

            scope.callBackMSDAsync = function (options) {
                // Node that we got the url from the options. Not necessary, but then the same callback function can be used
                // by different selects with different parameters.
                return http.get(options.urlOrWhateverOptionIWant);
            };

            scope.stringOptionsCallback = function (options) {
                // Here you can manipulate the form options used in a http_post or http_get
                // For example, you can use variables to build the URL or set the parameters, here we just set the url.
                options.httpPost.url = "test/testdata.json";
                // Note: This is a copy of the form options, edits here will not persist but are only used in this request.
                return options;
            };


            scope.schema = {
                type: 'object',
                title: 'Select',
                properties: {
                    select: {
                        title: 'Single Select Static',
                        type: 'string',
                        description: 'Only single item is allowed'
                    },
                    multiselect: {
                        title: 'Multi Select Static',
                        type: 'array',
                        items: {
                            type: "string"
                        },
                        maxItems: 2,
                        description: 'Multi single items are allowed. (select three for maxItems error)'
                    },
                    selectDynamic: {
                        title: 'Single Select Dynamic',
                        type: 'string',
                        items: {
                            type: "string"
                        },
                        description: 'This data is loaded from the $scope.callBackSD function. (and laid out using css-options)'
                    },
                    multiselectDynamic: {
                        title: 'Multi Select Dynamic',
                        type: 'array',
                        items: {
                            type: "string"
                        },
                        description: 'This data is loaded from the $scope.callBackMSD function. (referenced by name)'
                    },
                    multiselectDynamicHttpPost: {
                        title: 'Multi Select Dynamic HTTP Post',
                        type: 'array',
                        items: {
                            type: "string"
                        },
                        description: 'This data is asynchronously loaded using a HTTP post. ' +
                        '(specifies parameter in form, options.url in a named callback)'
                    },
                    multiselectDynamicHttpGet: {
                        title: 'Multi Select Dynamic HTTP Get',
                        type: 'array',
                        items: {
                            type: "string"
                        },
                        description: 'This data is asynchronously loaded using a HTTP get. ' +
                        '(Set the URL at options.url)'
                    },
                    multiselectDynamicHttpGetMapped: {
                        title: 'Multi Select Dynamic HTTP Get Mapped data',
                        type: 'array',
                        items: {
                            type: "string"
                        },
                        description: 'This data is as above, but remapped from a nodeId/nodeName array of objects. ' +
                        '(See app.js: "map" : {valueProperty: "nodeId", textProperty: "nodeName"})'
                    },
                    multiselectDynamicAsync: {
                        title: 'Multi Select Dynamic Async',
                        type: 'array',
                        items: {
                            type: "string"
                        },
                        description: 'This data is asynchrously loaded using a async call. ' +
                        '(specify options.async.call)'
                    }
                },
                required: ['select', 'multiselect']
            };

            scope.testResponse = [
                {value: "json-value1", name: "json-name1"},
                {value: "json-value2", name: "json-name2"},
                {value: "json-value3", name: "json-name3"}
            ];
            scope.testResponseMapped = [
                {"nodeId": "1", "nodeName": "Node 1"},
                {"nodeId": "2", "nodeName": "Node 2"},
                {"nodeId": "3", "nodeName": "Node 3"}
            ];
            scope.testResponseMappedCmp = [
                {"nodeId": "1", "nodeName": "Node 1", "value": "1", "name": "Node 1"},
                {"nodeId": "2", "nodeName": "Node 2", "value": "2", "name": "Node 2"},
                {"nodeId": "3", "nodeName": "Node 3", "value": "3", "name": "Node 3"}
            ];
            scope.form = [
                {
                    "key": 'select',
                    "type": 'strapselect',
                    "titleMap": [
                        {"value": 'value1', "name": 'text1'},
                        {"value": 'value2', "name": 'text2'},
                        {"value": 'value3', "name": 'text3'}
                    ]
                },
                {
                    "key": 'multiselect',
                    "type": 'strapmultiselect',
                    "titleMap": [
                        {"value": 'value1', "name": 'text1'},
                        {"value": 'value2', "name": 'text2'},
                        {"value": 'value3', "name": 'long very very long label3'}
                    ]
                },
                {
                    "key": "selectDynamic",
                    "type": 'strapselectdynamic',
                    "htmlClass": "col-lg-3 col-md-3",
                    "labelHtmlClass": "bigger",
                    "fieldHtmlClass": "tilted",
                    "options": {
                        "callback": scope.callBackSD
                    }
                },
                {
                    "key": "multiselectDynamic",
                    "type": 'strapmultiselectdynamic',
                    placeholder: "not set yet(this text is defined using the placeholder option)",
                    "options": {
                        "callback": "callBackMSD"
                    }
                },
                {
                    "key": "multiselectDynamicHttpPost",
                    "type": 'strapmultiselectdynamic',
                    "title": 'Multi Select Dynamic HTTP Post (title is from form.options, overriding the schema.title)',
                    "options": {
                        "httpPost": {
                            "optionsCallback": "stringOptionsCallback",
                            "parameter": {"myparam": "Hello"}
                        }
                    }
                },
                {
                    "key": "multiselectDynamicHttpGet",
                    "type": 'strapmultiselectdynamic',
                    "options": {
                        "httpGet": {
                            "url": "test/testdata.json"
                        }
                    }
                },
                {
                    "key": "multiselectDynamicHttpGetMapped",
                    "type": 'strapmultiselectdynamic',
                    "options": {
                        "httpGet": {
                            "url": "test/testdata_mapped.json"
                        },
                        "map": {valueProperty: "nodeId", nameProperty: "nodeName"}
                    }
                },
                {
                    "key": "multiselectDynamicAsync",
                    "type": 'strapmultiselectdynamic',
                    "onChange": function () {
                        alert("You changed this value! (this was the onChange event in action)");
                    },
                    "options": {
                        "asyncCallback": scope.callBackMSDAsync,
                        "urlOrWhateverOptionIWant": "test/testdata.json"
                    }
                }
            ];

            scope.model = {};
            scope.model.select = 'value1';
            scope.model.multiselect = ['value2', 'value1'];
            scope.model.multiselectDynamicHttpPost = null;
        };

        it('should load the correct items into each type of select', function () {
            inject(function ($compile, $rootScope, schemaForm, $http, $httpBackend, $timeout, $document) {
                var scope = $rootScope.$new();
                // Load example data
                assignToScope(scope, $http);

                // Create a template
                var tmpl = angular.element('<form sf-schema="schema" sf-form="form" sf-model="model"></form>');

                // Add http mocks
                $httpBackend.whenGET("test/testdata.json").respond(200, scope.testResponse);
                $httpBackend.whenGET("test/testdata_mapped.json").respond(200, scope.testResponseMapped);
                $httpBackend.whenPOST("test/testdata.json", {"myparam": "Hello"}).respond(200, scope.testResponse);

                // Compile the template
                $compile(tmpl)(scope);

                // Do an update, this triggers all items to be loaded
                $rootScope.$apply();

                // Attempt to click one of the selects, doesn't work as Karma seem to not work that way
                tmpl.children().eq(7).children().eq(0).children().eq(1).click();

                // Tell the mock to respond to requests
                $httpBackend.flush();

                // Wait for all getting done before checking.
                $timeout(function () {

                        // Find HTML elements in the response, find its scope, and then deep compare with known results.

                        // Single Select Dynamic
                        expect(JSON.stringify(angular.element(tmpl.children().eq(2).children().eq(0).children().eq(1)).scope().form.titleMap)).
                            to.equal(JSON.stringify(scope.callBackSD()), "Single Select Dynamic test failed.");
                        // Multi Select Dynamic
                        expect(JSON.stringify(angular.element(tmpl.children().eq(3).children().eq(0).children().eq(1)).scope().form.titleMap)).
                            to.equal(JSON.stringify(scope.callBackMSD()), "Multi Select Dynamic test failed.");
                        // Multi Select Dynamic HTTP Post
                        expect(JSON.stringify(angular.element(tmpl.children().eq(4).children().eq(0).children().eq(1)).scope().form.titleMap)).
                            to.equal(JSON.stringify(scope.testResponse), "Multi Select Dynamic HTTP Post test failed.");
                        // Multi Select Dynamic HTTP Get
                        expect(JSON.stringify(angular.element(tmpl.children().eq(5).children().eq(0).children().eq(1)).scope().form.titleMap)).
                            to.equal(JSON.stringify(scope.testResponse), "Multi Select Dynamic HTTP Get test failed.");
                        // Multi Select Dynamic HTTP Get Mapped
                        expect(JSON.stringify(angular.element(tmpl.children().eq(6).children().eq(0).children().eq(1)).scope().form.titleMap)).
                            to.equal(JSON.stringify(scope.testResponseMappedCmp), "Multi Select Dynamic HTTP Get Mapped test failed.");
                        // Multi Select Dynamic Async
                        expect(JSON.stringify(angular.element(tmpl.children().eq(7).children().eq(0).children().eq(1)).scope().form.titleMap)).
                            to.equal(JSON.stringify(scope.testResponse), "Multi Select Dynamic Async test failed.");

                    }
                );

                // Angular doesn't like async unit tests, tell it to call the checks above
                $timeout.flush()

            });
        });

    });
});
