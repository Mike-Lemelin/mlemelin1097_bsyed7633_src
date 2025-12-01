/* PROGRAMMER:  Michael Lemelin
 * FILE:        MainForm.cs
 * DATE:        2025-11-28
 * DESCRIPTION: This file contains the main form code that handles the core functionality of the
 *              control panel. Handles subscribing devices to selected topics, and unsubscribing devices.
 */

using MQTTnet;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static control_panel.MqttConstants;

namespace control_panel
{
    public partial class MainForm : Form
    {
        private IMqttClient? _client;
        private MqttClientFactory _mqttFactory;

        public MainForm()
        {
            InitializeComponent();
            _mqttFactory = new MqttClientFactory();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            topicList.View = View.Details;
            topicList.FullRowSelect = true;
            topicList.GridLines = true;
            topicList.Columns.Clear();
            topicList.Columns.Add("Topics", 1000);

            foreach (var device in Devices)
            {
                deviceCombo.Items.Add(device.Key);
            }

            deviceCombo.SelectedIndex = 0;

            subscribeBtn.Enabled = false;
            unsubBtn.Enabled = false;
            disconnectBtn.Enabled = false;
        }

        /// <summary>
        /// Subscribes the MQTT client to the base topic and registers a handler
        /// to receive all incoming MQTT messages from SENG3030/#. When a message comes in, the
        /// message's topic is added to the topic list.
        /// </summary>
        /// <returns></returns>
        private async Task ReceiveMessages()
        {
            if (_client == null)
            {
                MessageBox.Show("Client has not been established.", "Client Error");
                return;
            }

            _client.ApplicationMessageReceivedAsync += e =>
            {
                string topic = e.ApplicationMessage.Topic;

                Invoke((MethodInvoker)(() =>
                {
                    // Avoid duplicates
                    bool exists = topicList.Items.Cast<ListViewItem>().Any(item => item.Text == topic);

                    if (!exists)
                    {
                        // Insert at top of list
                        topicList.Items.Insert(0, new ListViewItem(topic));
                    }
                }));

                return Task.CompletedTask;
            };


            var mqttSubscribeOptions = _mqttFactory.CreateSubscribeOptionsBuilder().WithTopicFilter(BaseTopic).Build();
            await _client.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
        }

        /// <summary>
        /// Handles the Connect button click event and creates an MQTT connection
        /// using the configured broker settings, credentials, and CA certificate.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void connectBtn_Click(object sender, EventArgs e)
        {
            var caChain = new X509Certificate2Collection();
            caChain.ImportFromPem(CaCert);

            _client = _mqttFactory.CreateMqttClient();
            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(Broker, Port)
                .WithCredentials(Username, Password)
                .WithTlsOptions(new MqttClientTlsOptionsBuilder()
                    .WithTrustChain(caChain).Build())
                .Build();

            var connAck = await _client.ConnectAsync(mqttClientOptions);
            if (connAck.ResultCode != MqttClientConnectResultCode.Success)
            {
                MessageBox.Show("Connection failed: " + connAck.ResultCode);
                return;
            }

            subscribeBtn.Enabled = true;
            unsubBtn.Enabled = true;
            disconnectBtn.Enabled = true;
            connectBtn.Enabled = false;

            await ReceiveMessages();
        }

        /// <summary>
        /// Handles the Disconnect button click event by disconnecting from the
        /// MQTT broker, clearing UI elements, and updating the
        /// control states to show the disconnected status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void disconnectBtn_Click(object sender, EventArgs e)
        {
            await _client.DisconnectAsync(MqttClientDisconnectOptionsReason.NormalDisconnection);
            topicList.Items.Clear();
            selectedTopicText.Text = "";
            subscribeBtn.Enabled = false;
            unsubBtn.Enabled = false;
            disconnectBtn.Enabled = false;
            connectBtn.Enabled = true;
            MessageBox.Show("Disconnected from server.");
        }

        /// <summary>
        /// Sets the device topic text when the device combo box selection changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deviceCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            deviceTopicText.Text = Devices.ElementAt(deviceCombo.SelectedIndex).Value;
        }

        /// <summary>
        /// Sets the selected topic text when the user selects a topic from the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void topicList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (topicList.SelectedItems.Count == 0 || topicList.SelectedItems.Count > 1) { return; }
            ListViewItem selectedItem = topicList.SelectedItems[0];
            string topic = selectedItem.Text; // Get topic path
            selectedTopicText.Text = topic;
        }

        /// <summary>
        /// Handles the Subscribe button click event by validating user selections
        /// and publishing a subscription instruction to the selected device.
        /// This sends the chosen MQTT topic to the device control topic,
        /// allowing the device to begin listening to that topic.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void subscribeBtn_Click(object sender, EventArgs e)
        {
            // Check if options have been selected correctly
            if (deviceCombo.SelectedItem == null)
            {
                MessageBox.Show("No device selected for subscription.");
                return;
            }
            else if (topicList.SelectedItems.Count != 1)
            {
                MessageBox.Show("Incorrect number of topics selected.");
                return;
            }
            else if (_client == null)
            {
                MessageBox.Show("Client has not been established.");
                return;
            }

            // Subscribe device to topic selected
            string deviceTopic = Devices.ElementAt(deviceCombo.SelectedIndex).Value + Subscribe;
            string payload = topicList.SelectedItems[0].Text;

            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(deviceTopic)
                .WithPayload(payload)
                .Build();

            await _client.PublishAsync(applicationMessage, CancellationToken.None);
        }

        /// <summary>
        /// Handles the Unsubscribe button click event by validating the selected device
        /// and publishing an unsubscribe instruction to that device.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void unsubBtn_Click(object sender, EventArgs e)
        {
            if (deviceCombo.SelectedItem == null)
            {
                MessageBox.Show("No device selected for subscription.");
                return;
            }
            else if (_client == null)
            {
                MessageBox.Show("Client has not been established.");
                return;
            }

            // Send unsubscribe message
            string deviceTopic = Devices.ElementAt(deviceCombo.SelectedIndex).Value + Unsubscribe;

            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(deviceTopic)
                .Build();

            await _client.PublishAsync(applicationMessage, CancellationToken.None);
        }
    }
}
