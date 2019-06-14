package com.company.assembleegameclient.screens
{
import com.company.assembleegameclient.account.ui.Frame;
import com.company.rotmg.graphics.Loading;
import com.company.rotmg.graphics.TextFrame;

import flash.display.Sprite;
import flash.events.Event;
import flash.filters.DropShadowFilter;
import flash.text.TextFieldAutoSize;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;




public class AccountLoadingScreen extends Sprite
{
    private var loadingTextFrame:TextFrame;
    private var loadingText_:TextFieldDisplayConcrete;

    public function AccountLoadingScreen()
    {

        addChild(new Loading());

        this.loadingTextFrame = new TextFrame();
        addChild(this.loadingTextFrame);
        this.loadingText_ = new TextFieldDisplayConcrete().setSize(22).setColor(0xFFFFFF).setBold(true);
        this.loadingText_.setStringBuilder(new LineBuilder().setParams("Initalizing..."));
        this.loadingText_.filters = [new DropShadowFilter(0, 0, 0, 0, 0, 0, 0, 0)];
        addChild(this.loadingText_);

        addEventListener(Event.ADDED_TO_STAGE, this.onAddedToStage);
    }

    protected function onAddedToStage(_arg1:Event):void
    {
        removeEventListener(Event.ADDED_TO_STAGE, this.onAddedToStage);

        this.loadingTextFrame.x = ((stage.width / 2) - (this.loadingTextFrame.width / 2));
        this.loadingTextFrame.y = ((stage.height / 2) - (this.loadingTextFrame.height / 2));

        this.loadingText_.setAutoSize(TextFieldAutoSize.CENTER).setVerticalAlign(TextFieldDisplayConcrete.MIDDLE);
        this.loadingText_.x = ((this.loadingTextFrame.x) + (this.loadingTextFrame.width / 2) - (this.loadingText_.width / 2));
        this.loadingText_.y = ((this.loadingTextFrame.y) + (this.loadingTextFrame.height / 2) - (this.loadingText_.height / 2));
    }
}
}
