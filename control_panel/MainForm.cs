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
        /// 
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
        /// 
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
        /// 
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deviceCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            deviceTopicText.Text = Devices.ElementAt(deviceCombo.SelectedIndex).Value;
        }

        /// <summary>
        /// 
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
        /// 
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
            string deviceTopic = Devices.ElementAt(deviceCombo.SelectedIndex).Value + "subscribe";
            string payload = topicList.Items[0].Text;

            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(deviceTopic)
                .WithPayload(payload)
                .Build();

            await _client.PublishAsync(applicationMessage, CancellationToken.None);
        }

        /// <summary>
        /// 
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
            string deviceTopic = Devices.ElementAt(deviceCombo.SelectedIndex).Value + "unsubscribe";

            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(deviceTopic)
                .Build();

            await _client.PublishAsync(applicationMessage, CancellationToken.None);
        }
    }
}
