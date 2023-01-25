﻿///<reference path="../typings/knockout.d.ts" />
///<reference path="../typings/jquery.d.ts" />

interface IDraggableListBinding {
    maxItemsCount: KnockoutObservable<number>;
    onItemDropped: Function;
    groupName: KnockoutObservable<string>;
    allowedOperations: "All" | "Reorder" | "MoveToAnotherList";
}

class DraggableList {

    constructor(private $element: JQuery, private binding: IDraggableListBinding, private allBindings: any) {
    }

    initialize() {
        this.$element.on("dragover", e => this.onDragOver(e));
        this.$element.on("drop", e => this.onDrop(e));

        // make all newly added children draggable
        document.addEventListener("DOMNodeInserted", ev => {
            if (ev.target["nodeType"] === 1 && ev.target["parentElement"] === this.$element[0]) {
                this.initChildEvents(ev.target);
            }
        });
    }

    private initChildEvents(child: any) {
        $(child)
            .attr("draggable", "true")
            .off("drag")
            .on("drag", e => this.onDrag(e));
    }

    private static draggedItemSourceCollection: KnockoutObservableArray<any>;
    private static draggedItemIndex: number;
    private static draggedItemGroupName: string;
    private static draggedList: DraggableList;
    private static dragPositionIndicator: JQuery;
    private static dragConfirmed: boolean;
    private static dragLeaving: boolean;

    private static i: number = 0;

    private onDragOver(e: JQueryEventObject) {
        if (ko.unwrap(this.binding.maxItemsCount) > 0 && this.$element.children().length >= ko.unwrap(this.binding.maxItemsCount)) {
            return;
        }
        if (DraggableList.draggedItemGroupName !== ko.unwrap(this.binding.groupName)) {
            return;
        }
        if (DraggableList.draggedList.binding.allowedOperations === "Reorder" && DraggableList.draggedList !== this) {
            return;
        }
        if (DraggableList.draggedList.binding.allowedOperations === "MoveToAnotherList" && DraggableList.draggedList === this) {
            return;
        }

        e.preventDefault();
        DraggableList.dragLeaving = false;

        // get nearest target place and position the indicator
        var data = this.findChildByY(e.originalEvent["pageY"]);
        this.createDragPositionIndicator(data);
    }

    private onDrop(e: JQueryEventObject) {
        DraggableList.dragConfirmed = true;
        e.preventDefault();
        
        // get nearest target place 
        var data = this.findChildByY(e.originalEvent["pageY"]);
        console.log(data)
        var draggedItem = ko.unwrap(DraggableList.draggedItemSourceCollection)[DraggableList.draggedItemIndex];
        DraggableList.draggedItemSourceCollection.splice(DraggableList.draggedItemIndex, 1);

        if (data.index <= DraggableList.draggedItemIndex) {
            this.getDataSource().splice(data.index, 0, draggedItem);
        }
        else if (data.index > DraggableList.draggedItemIndex) {
            if (DraggableList.draggedItemSourceCollection === this.getDataSource()) {
                data.index--;
            }
            this.getDataSource().splice(data.index, 0, draggedItem);
        }

        // call the event
        if (this.binding.onItemDropped) {
            var target = this.getChildren()[data.index];
            this.binding.onItemDropped(target);
        }

        // reset
        DraggableList.onDragLeave(e);
    }
    private findChildByY(y: number): IndicatorPlacement {
        var children = this.getChildren();
        var offset = { left: 0, top: 0 };
        var width = 100;
        var height = 0;

        for (var i = 0; i < children.length; i++) {
            offset = $(children[i]).offset();
            width = $(children[i]).outerWidth();
            height = $(children[i]).outerHeight();

            if (y < offset.top + height / 2) {
                return {
                    index: i,
                    child: $(children[i]),
                    append: false,
                    x: offset.left,
                    width: width,
                    y: offset.top
                };
            }
        }
        return {
            index: children.length,
            child: children.length > 0 ? $(children[children.length - 1]) : null,
            append: true,
            x: offset.left,
            width: width,
            y: offset.top + height
        };
    }
    private static removeDragPositionIndicator() {
        if (DraggableList.dragPositionIndicator) {
            DraggableList.dragPositionIndicator.remove();
            DraggableList.dragPositionIndicator = null;
        }
    }

    private createDragPositionIndicator(data: IndicatorPlacement) {
        var created = false;
        if (!DraggableList.dragPositionIndicator) {
            DraggableList.dragPositionIndicator = $("<div class='draggable-list-indicator'></div>");
            created = true;
        }

        if (data.child == null) {
            this.$element.append(DraggableList.dragPositionIndicator);
        } else if (data.append) {
            DraggableList.dragPositionIndicator.insertAfter(data.child);
        } else {
            DraggableList.dragPositionIndicator.insertBefore(data.child);
        }

        if (created) {
            DraggableList.dragPositionIndicator.on("dragover", e => this.onDragOver(e));
        }
    }

    private getDataSource(): KnockoutObservableArray<any> {
        return this.allBindings.get("foreach");
    }

    private getChildren(): JQuery {
        var output = this.$element.children().not(".draggable-list-indicator");
        return output;
    }

    

    public onDrag(e: JQueryEventObject) {
        DraggableList.draggedItemIndex = ko.contextFor(<HTMLElement>e.target).$index();
        DraggableList.draggedItemSourceCollection = this.getDataSource();

        console.log(DraggableList.draggedItemSourceCollection)


        DraggableList.draggedItemGroupName = ko.unwrap(this.binding.groupName);
        DraggableList.draggedList = this;
        DraggableList.dragConfirmed = false;

    }

    public static getDataSourceFromExpression(viewModel: any) {
        var value = ko.unwrap(viewModel);
        if (typeof value === "undefined" || value === null) {
            return ko.observableArray([]);
        }
        return value.Items || viewModel;
    }

    public static onDragLeave(e: JQueryEventObject) {
        DraggableList.draggedItemGroupName = "";
        DraggableList.draggedItemIndex = -1;
        DraggableList.draggedItemSourceCollection = null;
        DraggableList.draggedList = null;
        DraggableList.dragConfirmed = false;
        DraggableList.removeDragPositionIndicator();
    }

    public static onDragLeaveCore(e: JQueryEventObject) {
        DraggableList.dragLeaving = true;
        window.setTimeout(() => {
            if (DraggableList.dragLeaving) {
                DraggableList.onDragLeave(e);
                DraggableList.dragLeaving = false;
            }
        }, 500);
    }
}

class IndicatorPlacement {
    public index: number;
    public child: JQuery;
    public append: boolean;
    public x: number;
    public width: number;
    public y: number;
}

ko.bindingHandlers["draggable-list"] = 
{
    init: function (element, valueAccessor, allBindingsAccessor) {
        var $element = $(element);

        var control = new DraggableList($element, valueAccessor(), allBindingsAccessor);
        $element.data("draggableList", control);

        control.initialize();
    },
    update: function (element, valueAccessor, allBindingsAccessor) {
    }
};
$(document).on("dragleave", e => DraggableList.onDragLeaveCore(e));
