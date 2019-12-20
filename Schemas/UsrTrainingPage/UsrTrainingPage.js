define("UsrTrainingPage", ["ServiceHelper"], function(ServiceHelper) {
	return {
		entitySchemaName: "UsrTraining",
		attributes: {
			"VirtualSum": {
				dataValueType: this.Terrasoft.DataValueType.FLOAT,
				caption: "Virtual Sum",
				value : 0.00
			},

			"UsrNameAttribute"	: {
				dependencies: [
                    {
                        // The value in the [UsrName] column depends on the [UsrFirstName,UsrLastName] 
                        // and [PaymentAmount] columns.
                        columns: ["UsrFirstName", "UsrLastName"],
                        // Handler method, which is called when changes are observed on ["UsrFirstName", "UsrLastName"]
                        methodName: "composeName"
                    }
                ]
			}
		},
		modules: /**SCHEMA_MODULES*/{}/**SCHEMA_MODULES*/,
		details: /**SCHEMA_DETAILS*/{
			"Files": {
				"schemaName": "FileDetailV2",
				"entitySchemaName": "UsrTrainingFile",
				"filter": {
					"masterColumn": "Id",
					"detailColumn": "UsrTraining"
				}
			},
			"UsrSchema1Detail954b4b86": {
				"schemaName": "UsrSchema1Detail",
				"entitySchemaName": "UsrTrainingParticipants",
				"filter": {
					"detailColumn": "UsrTraining",
					"masterColumn": "Id"
				}
			}
		}/**SCHEMA_DETAILS*/,
		businessRules: /**SCHEMA_BUSINESS_RULES*/{}/**SCHEMA_BUSINESS_RULES*/,
		methods: {
			
			init: function(){
				this.callParent(arguments);
				this.subscribeServerChannelEvents();
			},
			
			onEntityInitialized: function() {
				this.callParent(arguments);
				this.calcSumOfFees();
			},

			subscribeServerChannelEvents: function() {
				this.Terrasoft.ServerChannel.on(this.Terrasoft.EventName.ON_MESSAGE, this.onServerChannelMessageReceived, this);
			},
			
			unsubscribeServerChannelEvents: function() {
				this.Terrasoft.ServerChannel.un(this.Terrasoft.EventName.ON_MESSAGE, this.onServerChannelMessageReceived, this);
			},

			
			onServerChannelMessageReceived: function(sender, message) {
				
				this.showInformationDialog(message.Body);
			},

			composeName : function(){
				var result = this.get("UsrFirstName")+" "+this.get("UsrLastName");
				this.set("UsrName", result);
			},
			onMyButtonClick: function(){
				var firstName = this.$UsrFirstName;
                    // Object initializing incoming parameters for the service method.
                    var serviceData = {
                        // The property name corresponds to the incoming parameter name of the service method.
                        person: {
							FirstName: firstName,
							LastName : "Krylov"
						}
                    };
                    // Calling the web service and processing the results.
                    ServiceHelper.callService("DemoWs", "socketSample",
                        function(response) {
                            var result = response.PostMethodNameResult;
                            //this.showInformationDialog(result);
                        }, serviceData, this);
                },
			
			onDetailChanged: function() {
				this.callParent(arguments);
				this.calcSumOfFees();
			},
			
			calcSumOfFees: function(){
				var esq = this.Ext.create(
					"Terrasoft.EntitySchemaQuery", {
						rootSchemaName: "UsrTrainingParticipants"
					}
				);
				esq.addColumn("UsrFee");
				
				var recordId = this.$Id;
				var esqFirstFilter = esq.createColumnFilterWithParameter(Terrasoft.ComparisonType.EQUAL, "UsrTraining", recordId);
				esq.filters.add("esqFirstFilter", esqFirstFilter);
				
				var sumFee = 0.00;
				
				esq.getEntityCollection(function (result) {
				    if (result.success) {
				        result.collection.each(function (item) {
				            sumFee = sumFee + item.get("UsrFee");
				        });
				        this.set("VirtualSum", sumFee);
				    }
				}, this);

				
			}
		},
		dataModels: /**SCHEMA_DATA_MODELS*/{}/**SCHEMA_DATA_MODELS*/,
		diff: /**SCHEMA_DIFF*/[
			{
				"operation": "insert",
				"name": "MainContactButton",
				"values": {
					"itemType": 5,
					"style": "green",
					"caption": "My Button",
					"click": {
						"bindTo": "onMyButtonClick"
					},
					"layout": {
						"column": 1,
						"row": 6,
						"colSpan": 1
					}
				},
				"parentName": "LeftContainer",
				"propertyName": "items",
				"index": 7
			},
			{
				"operation": "insert",
				"name": "UsrName",
				"values": {
					"layout": {
						"colSpan": 24,
						"rowSpan": 1,
						"column": 0,
						"row": 0,
						"layoutName": "ProfileContainer"
					},
					"bindTo": "UsrName"
				},
				"parentName": "ProfileContainer",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "UsrFirstName",
				"values": {
					"layout": {
						"colSpan": 24,
						"rowSpan": 1,
						"column": 0,
						"row": 1,
						"layoutName": "ProfileContainer"
					},
					"bindTo": "UsrFirstName",
					"enabled": true
				},
				"parentName": "ProfileContainer",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "UsrLastName",
				"values": {
					"layout": {
						"colSpan": 24,
						"rowSpan": 1,
						"column": 0,
						"row": 2,
						"layoutName": "ProfileContainer"
					},
					"bindTo": "UsrLastName",
					"enabled": true
				},
				"parentName": "ProfileContainer",
				"propertyName": "items",
				"index": 2
			},
			{
				"operation": "insert",
				"name": "UsrSum",
				"values": {
					"layout": {
						"colSpan": 24,
						"rowSpan": 1,
						"column": 0,
						"row": 3,
						"layoutName": "ProfileContainer"
					},
					"bindTo": "VirtualSum"
				},
				"parentName": "ProfileContainer",
				"propertyName": "items",
				"index": 3
			},
			{
				"operation": "insert",
				"name": "Tab298cd28aTabLabel",
				"values": {
					"caption": {
						"bindTo": "Resources.Strings.Tab298cd28aTabLabelTabCaption"
					},
					"items": [],
					"order": 0
				},
				"parentName": "Tabs",
				"propertyName": "tabs",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "UsrSchema1Detail954b4b86",
				"values": {
					"itemType": 2,
					"markerValue": "added-detail"
				},
				"parentName": "Tab298cd28aTabLabel",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "NotesAndFilesTab",
				"values": {
					"caption": {
						"bindTo": "Resources.Strings.NotesAndFilesTabCaption"
					},
					"items": [],
					"order": 1
				},
				"parentName": "Tabs",
				"propertyName": "tabs",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "Files",
				"values": {
					"itemType": 2
				},
				"parentName": "NotesAndFilesTab",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "NotesControlGroup",
				"values": {
					"itemType": 15,
					"caption": {
						"bindTo": "Resources.Strings.NotesGroupCaption"
					},
					"items": []
				},
				"parentName": "NotesAndFilesTab",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "Notes",
				"values": {
					"bindTo": "UsrNotes",
					"dataValueType": 1,
					"contentType": 4,
					"layout": {
						"column": 0,
						"row": 0,
						"colSpan": 24
					},
					"labelConfig": {
						"visible": false
					},
					"controlConfig": {
						"imageLoaded": {
							"bindTo": "insertImagesToNotes"
						},
						"images": {
							"bindTo": "NotesImagesCollection"
						}
					}
				},
				"parentName": "NotesControlGroup",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "remove",
				"name": "ESNTab"
			},
			{
				"operation": "remove",
				"name": "ESNFeedContainer"
			},
			{
				"operation": "remove",
				"name": "ESNFeed"
			}
		]/**SCHEMA_DIFF*/
	};
});
