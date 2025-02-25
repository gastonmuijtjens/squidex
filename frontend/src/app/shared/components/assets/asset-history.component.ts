/*
 * Squidex Headless CMS
 *
 * @license
 * Copyright (c) Squidex UG (haftungsbeschränkt). All rights reserved.
 */

import { Component, Input } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AppsState, AssetDto, HistoryEventDto, HistoryService } from '@app/shared/internal';

interface AssetEvent { event: HistoryEventDto; version: number; canDownload: boolean }

@Component({
    selector: 'sqx-asset-history[asset]',
    styleUrls: ['./asset-history.component.scss'],
    templateUrl: './asset-history.component.html',
})
export class AssetHistoryComponent {
    @Input()
    public asset!: AssetDto;

    public assetEvents!: Observable<ReadonlyArray<AssetEvent>>;

    constructor(
        private readonly appsState: AppsState,
        private readonly historyService: HistoryService,
    ) {
    }

    public ngOnChanges() {
        const channel = `assets.${this.asset.id}`;

        this.assetEvents =
            this.historyService.getHistory(this.appsState.appName, channel).pipe(
                map(events => {
                    let version = -1;

                    return events.map(event => {
                        const canDownload =
                            event.eventType === 'AssetUpdatedEventV2' ||
                            event.eventType === 'AssetCreatedEventV2';

                        version++;

                        return { event, version, canDownload };
                    });
                }));
    }

    public trackByEvent(_index: number, assetEvent: AssetEvent) {
        return assetEvent.event.eventId;
    }
}
