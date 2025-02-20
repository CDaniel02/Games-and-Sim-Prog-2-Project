// Stolen directly from here: https://onemancrew.github.io/2013-01-10-csharp-notification-center/

using System;
using System.Collections;
using System.Collections.Generic;

public class NotificationCenter
{
    private static NotificationCenter m_instance;
    private readonly Hashtable m_hashtable;

    private NotificationCenter()
    {
        m_hashtable = new Hashtable();
    }
    /// <summary>
    /// Singelton Instance
    /// </summary>
    public static NotificationCenter Instance
    {
        get { return m_instance ?? (m_instance = new NotificationCenter()); }
    }
    /// <summary>
    /// Adds an entry to the receiver’s dispatch table with an observer, a notification Delegate and notification name.
    /// </summary>
    /// <param name="p_notificationDelegate">Delegate  that specifies the message the receiver sends notificationObserver to notify it of the notification posting</param>
    /// <param name="p_notificationName">The name of the notification for which to register the observer; that is, only notifications with this name are delivered to the observer</param>
    public void AddObserver(string p_notificationName, NotificationDelegate p_notificationDelegate)
    {
        if (string.IsNullOrEmpty(p_notificationName))
        {
            throw new ArgumentNullException(@"p_notificationDelegate");

        }
        if (p_notificationDelegate == null)
        {
            throw new ArgumentNullException("p_notificationDelegate");

        }
        var delegatesCollection = (List<NotificationDelegate>)m_hashtable[p_notificationName];
        if (delegatesCollection == null)
        {
            delegatesCollection = new List<NotificationDelegate>();
            m_hashtable.Add(p_notificationName, delegatesCollection);
        }
        delegatesCollection.Add(p_notificationDelegate);
    }
    /// <summary>
    /// Removes matching entries from the receiver’s dispatch table.
    /// </summary>
    /// <param name="p_notificationDelegate">Delegate  that specifies the message the receiver sends notificationObserver to notify it of the notification posting</param>
    /// <param name="p_notificationName">The name of the notification for which to register the observer; that is, only notifications with this name are delivered to the observer</param>
    public void RemoveObserver(string p_notificationName, NotificationDelegate p_notificationDelegate)
    {
        if (string.IsNullOrEmpty(p_notificationName))
        {
            throw new ArgumentNullException(@"p_notificationName");

        }
        if (p_notificationDelegate == null)
        {
            throw new ArgumentNullException("p_notificationDelegate");

        }
        var delegatesCollection = (List<NotificationDelegate>)m_hashtable[p_notificationName];
        if (delegatesCollection != null)
        {
            delegatesCollection.Remove(p_notificationDelegate);
        }
    }
    /// <summary>
    /// Creates a notification with a given name and sender and posts it to the receiver.
    /// </summary>
    /// <param name="p_notificationName">The name of the notification for which to register the observer; that is, only notifications with this name are delivered to the observer.</param>
    /// <param name="p_notification">The notification includinf the sender and a message.</param>
    public void PostNotification(Notification p_notification)
    {
        if (string.IsNullOrEmpty(p_notification.Name))
        {
            throw new ArgumentNullException(@"p_notificationName");

        }
        if (p_notification == null)
        {
            throw new ArgumentNullException("p_notification");

        }
        var delegatesCollection = (List<NotificationDelegate>)m_hashtable[p_notification.Name];
        if (delegatesCollection != null)
        {
            foreach (var notificationDelegate in delegatesCollection)
            {
                notificationDelegate(p_notification);
            }

        }

    }
    /// <summary>
    /// Creates a notification with a given name and sender and posts it to the receiver with Empty notification.
    /// </summary>
    /// <param name="p_notificationName">The name of the notification for which to register the observer; that is, only notifications with this name are delivered to the observer.</param>
    public void PostNotification(string p_notificationName)
    {
        if (string.IsNullOrEmpty(p_notificationName))
        {
            throw new ArgumentNullException(@"p_notificationName");

        }

        var delegatesCollection = (List<NotificationDelegate>)m_hashtable[p_notificationName];
        if (delegatesCollection != null)
        {
            foreach (var notificationDelegate in delegatesCollection)
            {
                try
                {
                    notificationDelegate(Notification.Empty);
                }
                catch (Exception)
                {

                    Console.WriteLine("Error fire Notification.");
                }

            }

        }

    }

    public delegate void NotificationDelegate(Notification p_notification);
}

