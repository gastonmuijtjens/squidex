/*
 * Squidex Headless CMS
 *
 * @license
 * Copyright (c) Squidex UG (haftungsbeschränkt). All rights reserved.
 */

import { lastValueFrom, NEVER, Observable, of, onErrorResumeNextWith, throwError } from 'rxjs';
import { IMock, Mock } from 'typemoq';
import { AssetsService, AssetUploaderState, DialogService } from '@app/shared/internal';
import { createAsset } from './../services/assets.service.spec';
import { TestValues } from './_test-helpers';

describe('AssetUploaderState', () => {
    const {
        app,
        appsState,
    } = TestValues;

    let assetsService: IMock<AssetsService>;
    let dialogs: IMock<DialogService>;
    let assetUploader: AssetUploaderState;

    const asset = createAsset(1);

    beforeEach(() => {
        dialogs = Mock.ofType<DialogService>();

        assetsService = Mock.ofType<AssetsService>();
        assetUploader = new AssetUploaderState(appsState.object, assetsService.object, dialogs.object);
    });

    afterEach(() => {
        assetsService.verifyAll();
    });

    it('should create initial state if uploading file', () => {
        const file: File = <any>{ name: 'my-file' };

        assetsService.setup(x => x.postAssetFile(app, file, undefined))
            .returns(() => NEVER).verifiable();

        assetUploader.uploadFile(file).subscribe();

        const upload = assetUploader.snapshot.uploads[0];

        expect(upload.status).toBe('Running');
        expect(upload.progress).toBe(1);
    });

    it('should update progress if uploading file makes progress', () => {
        const file: File = <any>{ name: 'my-file' };

        assetsService.setup(x => x.postAssetFile(app, file, undefined))
            .returns(() => ofForever(10, 20)).verifiable();

        assetUploader.uploadFile(file).subscribe();

        const upload = assetUploader.snapshot.uploads[0];

        expect(upload.status).toBe('Running');
        expect(upload.progress).toBe(20);
    });

    it('should update status if uploading file failed', () => {
        const file: File = <any>{ name: 'my-file' };

        assetsService.setup(x => x.postAssetFile(app, file, undefined))
            .returns(() => throwError(() => 'Service Error')).verifiable();

        assetUploader.uploadFile(file).pipe(onErrorResumeNextWith()).subscribe();

        const upload = assetUploader.snapshot.uploads[0];

        expect(upload.status).toBe('Failed');
        expect(upload.progress).toBe(1);
    });

    it('should update status if uploading file completes', async () => {
        const file: File = <any>{ name: 'my-file' };

        assetsService.setup(x => x.postAssetFile(app, file, undefined))
            .returns(() => of(10, 20, asset)).verifiable();

        const uploadedAsset = await lastValueFrom(assetUploader.uploadFile(file));

        const upload = assetUploader.snapshot.uploads[0];

        expect(upload.status).toBe('Completed');
        expect(upload.progress).toBe(100);
        expect(uploadedAsset).toEqual(asset);
    });

    it('should create initial state if uploading asset', () => {
        const file: File = <any>{ name: 'my-file' };

        assetsService.setup(x => x.putAssetFile(app, asset, file, asset.version))
            .returns(() => NEVER).verifiable();

        assetUploader.uploadAsset(asset, file).subscribe();

        const upload = assetUploader.snapshot.uploads[0];

        expect(upload.status).toBe('Running');
        expect(upload.progress).toBe(1);
    });

    it('should update progress if uploading asset makes progress', () => {
        const file: File = <any>{ name: 'my-file' };

        assetsService.setup(x => x.putAssetFile(app, asset, file, asset.version))
            .returns(() => ofForever(10, 20)).verifiable();

        assetUploader.uploadAsset(asset, file).subscribe();

        const upload = assetUploader.snapshot.uploads[0];

        expect(upload.status).toBe('Running');
        expect(upload.progress).toBe(20);
    });

    it('should update status if uploading asset failed', () => {
        const file: File = { name: 'my-file' } as any;

        assetsService.setup(x => x.putAssetFile(app, asset, file, asset.version))
            .returns(() => throwError(() => 'Service Error')).verifiable();

        assetUploader.uploadAsset(asset, file).pipe(onErrorResumeNextWith()).subscribe();

        const upload = assetUploader.snapshot.uploads[0];

        expect(upload.status).toBe('Failed');
        expect(upload.progress).toBe(1);
    });

    it('should update status if uploading asset completes', async () => {
        const file: File = { name: 'my-file' } as any;

        const updated = createAsset(1, undefined, '_new');

        assetsService.setup(x => x.putAssetFile(app, asset, file, asset.version))
            .returns(() => of(10, 20, updated)).verifiable();

        const uploadedAsset = await lastValueFrom(assetUploader.uploadAsset(asset, file));

        const upload = assetUploader.snapshot.uploads[0];

        expect(upload.status).toBe('Completed');
        expect(upload.progress).toBe(100);
        expect(uploadedAsset!).toEqual(updated);
    });
});

function ofForever<T>(...values: ReadonlyArray<T>) {
    return new Observable<T>(s => {
        for (const value of values) {
            s.next(value);
        }
    });
}