/*
 * Squidex Headless CMS
 *
 * @license
 * Copyright (c) Squidex UG (haftungsbeschränkt). All rights reserved.
 */

import { DragDropModule } from '@angular/cdk/drag-drop';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MentionModule } from 'angular-mentions';
import { NgChartsModule } from 'ng2-charts';
import { NgxDocViewerModule } from 'ngx-doc-viewer';
import { SqxFrameworkModule } from '@app/framework';
import { ApiCallsCardComponent, ApiCallsSummaryCardComponent, ApiPerformanceCardComponent, ApiTrafficCardComponent, ApiTrafficSummaryCardComponent, AppFormComponent, AppLanguagesService, AppMustExistGuard, AppsService, AppsState, AssetComponent, AssetDialogComponent, AssetFolderComponent, AssetFolderDialogComponent, AssetFolderDropdownComponent, AssetFolderDropdownItemComponent, AssetHistoryComponent, AssetPathComponent, AssetPreviewUrlPipe, AssetScriptsState, AssetsListComponent, AssetSelectorComponent, AssetsService, AssetsState, AssetTextEditorComponent, AssetUploaderComponent, AssetUploaderState, AssetUploadsCountCardComponent, AssetUploadsSizeCardComponent, AssetUploadsSizeSummaryCardComponent, AssetUrlPipe, AuthInterceptor, AuthService, AutoSaveService, BackupsService, BackupsState, ClientsService, ClientsState, CommentComponent, CommentsComponent, CommentsService, ContentListCellDirective, ContentListCellResizeDirective, ContentListFieldComponent, ContentListHeaderComponent, ContentListWidthDirective, ContentMustExistGuard, ContentsColumnsPipe, ContentSelectorComponent, ContentSelectorItemComponent, ContentsService, ContentsState, ContentStatusComponent, ContentValueComponent, ContentValueEditorComponent, ContributorsService, ContributorsState, FileIconPipe, FilterComparisonComponent, FilterLogicalComponent, FilterNodeComponent, FilterOperatorPipe, GeolocationEditorComponent, HelpComponent, HelpMarkdownPipe, HelpService, HistoryComponent, HistoryListComponent, HistoryMessagePipe, HistoryService, IFrameCardComponent, ImageCropperComponent, ImageFocusPointComponent, LanguagesService, LanguagesState, LoadAppsGuard, LoadLanguagesGuard, LoadSchemasGuard, LoadTeamsGuard, MarkdownEditorComponent, MustBeAuthenticatedGuard, MustBeNotAuthenticatedGuard, NewsService, NotifoComponent, PlansService, PlansState, PreviewableType, QueryComponent, QueryListComponent, QueryPathComponent, RandomCatCardComponent, RandomDogCardComponent, ReferenceInputComponent, RichEditorComponent, RolesService, RolesState, RuleEventsState, RuleMustExistGuard, RuleSimulatorState, RulesService, RulesState, SavedQueriesComponent, SchemaCategoryComponent, SchemaMustExistGuard, SchemaMustExistPublishedGuard, SchemaMustNotBeSingletonGuard, SchemasService, SchemasState, SchemaTagSource, SearchFormComponent, SearchService, SortingComponent, StockPhotoService, SupportCardComponent, TableHeaderComponent, TeamFormComponent, TeamMustExistGuard, TeamsService, TeamsState, TemplatesService, TemplatesState, TranslationsService, TranslationStatusComponent, UIService, UIState, UnsetAppGuard, UnsetTeamGuard, UsagesService, UserDtoPicture, UserIdPicturePipe, UserNamePipe, UserNameRefPipe, UserPicturePipe, UserPictureRefPipe, UsersProviderService, UsersService, WatchingUsersComponent, WorkflowsService, WorkflowsState } from './declarations';

@NgModule({
    imports: [
        DragDropModule,
        MentionModule,
        NgChartsModule,
        NgxDocViewerModule,
        RouterModule,
        SqxFrameworkModule,
    ],
    declarations: [
        ApiCallsCardComponent,
        ApiCallsSummaryCardComponent,
        ApiPerformanceCardComponent,
        ApiTrafficCardComponent,
        ApiTrafficSummaryCardComponent,
        AppFormComponent,
        AssetComponent,
        AssetDialogComponent,
        AssetFolderComponent,
        AssetFolderDialogComponent,
        AssetFolderDropdownComponent,
        AssetFolderDropdownItemComponent,
        AssetHistoryComponent,
        AssetPathComponent,
        AssetPreviewUrlPipe,
        AssetSelectorComponent,
        AssetsListComponent,
        AssetTextEditorComponent,
        AssetUploaderComponent,
        AssetUploadsCountCardComponent,
        AssetUploadsSizeCardComponent,
        AssetUploadsSizeSummaryCardComponent,
        AssetUrlPipe,
        CommentComponent,
        CommentsComponent,
        ContentListCellDirective,
        ContentListCellResizeDirective,
        ContentListFieldComponent,
        ContentListHeaderComponent,
        ContentListWidthDirective,
        ContentsColumnsPipe,
        ContentSelectorComponent,
        ContentSelectorItemComponent,
        ContentStatusComponent,
        ContentValueComponent,
        ContentValueEditorComponent,
        FileIconPipe,
        FilterComparisonComponent,
        FilterLogicalComponent,
        FilterNodeComponent,
        FilterOperatorPipe,
        GeolocationEditorComponent,
        HelpComponent,
        HelpMarkdownPipe,
        HistoryComponent,
        HistoryListComponent,
        HistoryMessagePipe,
        IFrameCardComponent,
        ImageCropperComponent,
        ImageFocusPointComponent,
        MarkdownEditorComponent,
        NotifoComponent,
        PreviewableType,
        QueryComponent,
        QueryListComponent,
        QueryPathComponent,
        RandomCatCardComponent,
        RandomDogCardComponent,
        ReferenceInputComponent,
        RichEditorComponent,
        SavedQueriesComponent,
        SchemaCategoryComponent,
        SearchFormComponent,
        SortingComponent,
        SupportCardComponent,
        TableHeaderComponent,
        TeamFormComponent,
        TranslationStatusComponent,
        UserDtoPicture,
        UserIdPicturePipe,
        UserNamePipe,
        UserNameRefPipe,
        UserPicturePipe,
        UserPictureRefPipe,
        WatchingUsersComponent,
    ],
    exports: [
        ApiCallsCardComponent,
        ApiCallsSummaryCardComponent,
        ApiPerformanceCardComponent,
        ApiTrafficCardComponent,
        ApiTrafficSummaryCardComponent,
        AppFormComponent,
        AssetComponent,
        AssetDialogComponent,
        AssetFolderComponent,
        AssetFolderDialogComponent,
        AssetFolderDropdownComponent,
        AssetPathComponent,
        AssetPreviewUrlPipe,
        AssetSelectorComponent,
        AssetsListComponent,
        AssetUploaderComponent,
        AssetUploadsCountCardComponent,
        AssetUploadsSizeCardComponent,
        AssetUploadsSizeSummaryCardComponent,
        AssetUrlPipe,
        CommentComponent,
        CommentsComponent,
        ContentListCellDirective,
        ContentListCellResizeDirective,
        ContentListFieldComponent,
        ContentListHeaderComponent,
        ContentListWidthDirective,
        ContentsColumnsPipe,
        ContentSelectorComponent,
        ContentSelectorItemComponent,
        ContentStatusComponent,
        ContentValueComponent,
        ContentValueEditorComponent,
        DragDropModule,
        FileIconPipe,
        GeolocationEditorComponent,
        HelpComponent,
        HelpMarkdownPipe,
        HistoryComponent,
        HistoryListComponent,
        HistoryMessagePipe,
        IFrameCardComponent,
        MarkdownEditorComponent,
        NotifoComponent,
        PreviewableType,
        QueryListComponent,
        RandomCatCardComponent,
        RandomDogCardComponent,
        ReferenceInputComponent,
        RichEditorComponent,
        RouterModule,
        SavedQueriesComponent,
        SchemaCategoryComponent,
        SearchFormComponent,
        SupportCardComponent,
        TableHeaderComponent,
        TeamFormComponent,
        TranslationStatusComponent,
        UserDtoPicture,
        UserIdPicturePipe,
        UserNamePipe,
        UserNameRefPipe,
        UserPicturePipe,
        UserPictureRefPipe,
        WatchingUsersComponent,
    ],
})
export class SqxSharedModule {
    public static forRoot(): ModuleWithProviders<SqxSharedModule> {
        return {
            ngModule: SqxSharedModule,
            providers: [
                AppLanguagesService,
                AppMustExistGuard,
                AppsService,
                AppsState,
                AssetScriptsState,
                AssetsService,
                AssetsState,
                AssetUploaderState,
                AuthService,
                AutoSaveService,
                BackupsService,
                BackupsState,
                ClientsService,
                ClientsState,
                CommentsService,
                ContentMustExistGuard,
                ContentsService,
                ContentsState,
                ContributorsService,
                ContributorsState,
                HelpService,
                HistoryService,
                LanguagesService,
                LanguagesState,
                LoadAppsGuard,
                LoadLanguagesGuard,
                LoadSchemasGuard,
                LoadTeamsGuard,
                MustBeAuthenticatedGuard,
                MustBeNotAuthenticatedGuard,
                NewsService,
                PlansService,
                PlansState,
                RolesService,
                RolesState,
                RuleEventsState,
                RuleMustExistGuard,
                RuleSimulatorState,
                RulesService,
                RulesState,
                SchemaMustExistGuard,
                SchemaMustExistPublishedGuard,
                SchemaMustNotBeSingletonGuard,
                SchemasService,
                SchemasState,
                SchemaTagSource,
                SearchService,
                StockPhotoService,
                TeamMustExistGuard,
                TemplatesService,
                TemplatesState,
                TeamsState,
                TeamsService,
                TranslationsService,
                UIService,
                UIState,
                UnsetAppGuard,
                UnsetTeamGuard,
                UsagesService,
                UsersProviderService,
                UsersService,
                WorkflowsService,
                WorkflowsState,
                {
                    provide: HTTP_INTERCEPTORS,
                    useClass: AuthInterceptor,
                    multi: true,
                },
            ],
        };
    }
}
