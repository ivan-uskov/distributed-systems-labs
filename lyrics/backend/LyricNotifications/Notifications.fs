namespace LyricNotifications

open RabbitMQClient
open LyricModel.Lyric

module Events =
    let prepareEventMessage t lyric =
        sprintf "{\"id\":\"%s\",\"text\":\"%s\",\"type\":\"%s\"}" lyric.id lyric.text t

    let handleUploadInitialized (lyric : Lyric) =
        let msg = prepareEventMessage "uploadInitialized" lyric
        Queue.publishMessage(Queue.STATISTICS_NOTIFICATIONS_QUEUE, msg)

    let handleUploadCompleted (lyric : Lyric) =
        let msg = prepareEventMessage "uploadCompleted" lyric
        Queue.publishMessage(Queue.STATISTICS_NOTIFICATIONS_QUEUE, msg)
        Queue.publishMessage(Queue.STORE_LYRIC_QUEUE, msg)

    let handleUploadFailed (lyric : Lyric) =
        let msg = prepareEventMessage "uploadFailed" lyric
        Queue.publishMessage(Queue.STATISTICS_NOTIFICATIONS_QUEUE, msg)
